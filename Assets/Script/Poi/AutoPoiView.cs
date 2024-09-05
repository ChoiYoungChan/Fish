using System.Collections;
using Fish;
using UnityEngine;
using Zenject;

namespace View
{
    public class AutoPoiView : MonoBehaviour
    {
        [SerializeField] private Transform heightBaseTrm;

        [Inject] private IPoiPresenter _poiPresenter;
        [Inject] private GameConfig _gameConfig;
        
        private PoiView _poiView;

        /// <summary>
        /// TriggerEnterで魚を捕まった時
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerConst.Fish)
            {
                StartCoroutine(nameof(UpCoroutine));
            }
        }

        /// <summary>
        /// Poiを上げる処理
        /// </summary>
        /// <returns></returns>
        IEnumerator UpCoroutine()
        {
            if (_poiView)
            {
                _poiPresenter.Up(_poiView.PoiType);
                yield return new WaitForSeconds(1.5f);
            }
            else  //_poiViewがいなかったら何もしない
            {
                yield return null;
            }
        }
    }
}