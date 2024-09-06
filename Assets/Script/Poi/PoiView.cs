using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using UniRx;
using Data;
using Manager;
using Fish;

namespace View
{
    public enum PoiType
    {
        Player
        // TODO: 今後マルチプレイのため追加予定
    }

    public interface IPoiInitialize
    {
        void SetPoiType(PoiType type);
    }

    public class PoiView : MonoBehaviour, IPoiInitialize
    {
        public PoiType PoiType => _poiType;

        [SerializeField] private int poiHp;
        [SerializeField] private float speed;
        [SerializeField] private List<Collider> colliders;
        [SerializeField] private GameObject normalNet;
        [SerializeField] private GameObject tearNet;
        [SerializeField] private GameObject splashPrefab;
        [SerializeField] private List<GameObject> poiForms;

        [Inject] private IPoiPresenter _poiPresenter;
        [Inject] private GameConfig _gameConfig;

        private GameObject _gotchaFish;
        private bool _willTear;
        private bool _canMove;
        private List<BaseFish> _fishViews = new();
        private Vector3 _defaultPos;
        private Vector3 _offset;
        private PoiType _poiType;

        public void SetPoiType(PoiType type) => _poiType = type;

        // Start is called before the first frame update
        void Start()
        {
            _poiPresenter.ObservableMove
                .Where(tuple => _poiType == tuple.poiType)
                .Subscribe(tuple => Move(tuple.targetPos))
                .AddTo(this);

            _poiPresenter.ObservableUp
                .Where(type => type == _poiType)
                .Subscribe(_ => Up())
                .AddTo(this);

            SwitchCollider(false);

            _defaultPos = transform.position;
            _offset = -transform.forward * 3f;
            transform.position += _offset;
            transform.DOMove(_defaultPos, 0.3f).OnComplete(() =>
            {
                _canMove = true;
            });
        }

        /// <summary>
        /// Poiを動かす関数
        /// </summary>
        /// <param name="targetPos"></param>
        private void Move(Vector3 targetPos)
        {
            if (!_canMove) return;

            transform.position = targetPos;
        }

        /// <summary>
        /// Poiを上に上げる関数
        /// </summary>
        private void Up()
        {
            if (PoiType == PoiType.Player)
            {
                SoundManager.Instance.PlaySe(SeType.Splash);
            }

            SwitchCollider(true);
            var point = new Vector3(transform.position.x, _defaultPos.y, transform.position.z);

            // 上げるのが終わる際の処理
            transform.DOMove(point, 0.1f)
                .OnComplete(() =>
                {
                    ShowSplash();
                    SwitchCollider(false);
                    if (_gotchaFish)
                    {
                        StartCoroutine(nameof(OnGotchaCoroutine));
                    }
                });
        }

        /// <summary>
        /// Splashエフェクトを出す関数
        /// </summary>
        private void ShowSplash()
        {
            var splash = Instantiate(splashPrefab);
            splash.transform.position = transform.position + Vector3.down * 0.3f;
        }

        /// <summary>
        /// PoiのColliderをtrue/false切り替える処理
        /// </summary>
        /// <param name="enable"></param>
        private void SwitchCollider(bool enable)
        {
            foreach (var collider in colliders)
            {
                if (collider.enabled != enable)
                {
                    collider.enabled = enable;
                }
            }
        }

        /// <summary>
        /// OnTriggerEnterで魚を捕まったのかを判断
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerConst.Fish)
                Gotcha(other.transform);
        }

        /// <summary>
        /// 魚を捕まる際の処理、Poiの耐久度も下げる
        /// </summary>
        /// <param name="fishTrm"></param>
        private void Gotcha(Transform fishTrm)
        {
            _gotchaFish = fishTrm.gameObject;
            var fishView = _gotchaFish.GetComponent<BaseFish>();

            if (_fishViews.Contains(fishView)) return;

            fishTrm.SetParent(transform, true);

            fishView.Gotcha();
            _willTear = WillTear(fishView.FishHP);
            Damaged(fishView.FishHP);

            _fishViews.Add(fishView);
        }

        /// <summary>
        /// 魚を捕まる際の処理をCoroutineで処理
        /// </summary>
        /// <returns></returns>
        IEnumerator OnGotchaCoroutine()
        {
            _canMove = false;
            // 破れないなら
            if (!_willTear)
            {
                yield return new WaitForSeconds(0.6f);
                _gotchaFish = null;
                _canMove = true;
                for (int count = 0; count < _fishViews.Count; count++)
                {
                    _fishViews[count].Collect();
                }
                _fishViews.Clear();
            }
            else　// 破れた際の処理
            {
                Tear();
                yield return new WaitForSeconds(0.2f);
                ReleaseFishes();
                yield return new WaitForSeconds(0.2f);
                RemoveAction();
            }
        }

        /// <summary>
        /// 破れるかチェック
        /// </summary>
        /// <param name="fishHp"></param>
        /// <returns></returns>
        private bool WillTear(int fishHp)
        {
            return poiHp <= fishHp;
        }

        /// <summary>
        /// 一度使う際に魚のHP分耐久度が下がる
        /// </summary>
        /// <param name="fishHp"></param>
        private void Damaged(int fishHp)
        {
            poiHp -= fishHp;
        }

        /// <summary>
        /// 破れる処理をする関数
        /// </summary>
        private void Tear()
        {
            normalNet.SetActive(false);
            tearNet.SetActive(true);
        }

        /// <summary>
        /// 破れたらPoiを画面外に隠して削除
        /// </summary>
        private void RemoveAction()
        {
            transform.DOMove(_defaultPos + _offset, 0.3f).OnComplete(() =>
            {
                _poiPresenter.Teared(_poiType);
                Destroy(gameObject);
            });
        }

        /// <summary>
        /// 魚を解放する関数
        /// </summary>
        private void ReleaseFishes()
        {
            for (int count = 0; count < _fishViews.Count; count++)
            {
                _fishViews[count].transform.SetParent(null);
                _fishViews[count].Release();
            }
            _fishViews.Clear();
        }
    }
}