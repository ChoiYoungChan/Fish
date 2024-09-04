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
        private bool _isActive;
        private float _baseHeight;


        private void Start()
        {
            _baseHeight = heightBaseTrm.position.y;
        }

        public void SetPoiView(PoiView poiView)
        {
            poiView.transform.position = transform.position;
            poiView.transform.SetParent(transform);
            _poiView = poiView;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerConst.Fish && _isActive)
            {
                StartCoroutine(nameof(UpCoroutine));
            }
        }

        IEnumerator UpCoroutine()
        {
            if (_poiView)
            {
                _poiPresenter.Up(_poiView.PoiType);
                yield return new WaitForSeconds(1.5f);
            }
            
            //_poiViewがいなかったら何もしない
            else
            {
                yield return null;
            }
        }
    }
}