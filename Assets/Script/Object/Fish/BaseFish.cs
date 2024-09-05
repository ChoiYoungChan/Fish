using System;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Zenject;
using Random = UnityEngine.Random;

namespace Data
{
    public enum FishType
    {
        GoldFish_Red,
        GoldFish_Black,
        GoldFish_Yellow,
        GoldFish_White,
        Turtle_Green,
        Turtle_Orange,
        Frog,
        Sardine,
        ClownFish,
        Squid_Purple,
        Squid_White,
        Arowana,
        Herring,
        Lobster_Red,
        Lobster_Blue,
        Prawn,
        Carp,
        Salmon
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public enum FishStatus
    {
        Idle,
        Swim,
        Gotcha
    }


    public class BaseFish : MonoBehaviour
    {
        [SerializeField] protected FishType FishType;
        [SerializeField] protected float _speed = 1f;
        [SerializeField] protected float _minIdleTime = 0.3f;
        [SerializeField] protected float _maxIdleTime = 2f;
        [SerializeField] protected Vector3 _gotchaOffsetAngle = new Vector3(0, 0, 90);

        [Inject] private IStagePresenter _stagePresenter;

        protected FishStatus _fishStatus;
        private Vector3 _gotchaPos;
        private Transform _defaultParent;

        public int FishHP = 1;


        private void Start()
        {
            Init();
            Idle();
            _defaultParent = transform.parent;
        }

        /// <summary>
        /// 動くターゲット座標を取得
        /// </summary>
        /// <returns></returns>
        private Vector3 GetTargetPoint()
        {
            var poolRandomPoint = PoolManager.Instance.GetRandomTargetPoint();
            poolRandomPoint.y = transform.position.y;
            return poolRandomPoint;
        }

        protected virtual void Init()
        {

        }

        /// <summary>
        /// 魚の状態変更
        /// </summary>
        /// <param name="status"></param>
        protected void ChangeStatus(FishStatus status)
        {
            _fishStatus = status;
        }

        /// <summary>
        /// 魚を動かす処理
        /// </summary>
        /// <param name="point"></param>
        protected virtual void Move(Vector3 point)
        {
            //魚が捕まれた状態なら return
            if (_fishStatus == FishStatus.Gotcha) return;

            var distance = Vector3.Distance(transform.position, point);
            transform.LookAt(point);
            transform.DOMove(point, distance / _speed).SetEase(Ease.OutCubic).OnComplete(() => Idle());
        }

        /// <summary>
        /// Idle状態
        /// </summary>
        protected virtual void Idle()
        {
            Observable.Timer(TimeSpan.FromSeconds(Random.Range(_minIdleTime, _maxIdleTime)))
                .Take(1)
                .Subscribe(_ => Move(GetTargetPoint()))
                .AddTo(this);
        }

        /// <summary>
        /// Poiに捕まれ時
        /// </summary>
        public virtual void Gotcha()
        {
            _gotchaPos = transform.position;
            ChangeStatus(FishStatus.Gotcha);
            transform.DOKill();
            transform.DOLocalRotate(transform.localEulerAngles + _gotchaOffsetAngle, 0.2f);
        }

        /// <summary>
        /// 魚を捕まる事のを成功した際
        /// </summary>
        public void Collect()
        {
            _stagePresenter.Gotcha(FishType);
            transform.DOScale(0f, 0.3f).SetLink(gameObject).OnComplete(() => { Destroy(gameObject); });
            transform.DOMove(new Vector3(0, -18.78f, 3.27f), 0.3f).SetLink(gameObject);
        }

        /// <summary>
        /// 魚を逃がした際
        /// </summary>
        public virtual void Release()
        {
            transform.SetParent(_defaultParent);
            transform.DOJump(_gotchaPos, 1f, 1, 0.5f).OnComplete(() =>
            {
                transform.DOLocalRotate(transform.localEulerAngles - _gotchaOffsetAngle, 0.2f);
                ChangeStatus(FishStatus.Idle);
                Idle();
            });
        }
    }
}