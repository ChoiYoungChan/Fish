using System;
using UnityEngine;
using Zenject;
using UniRx;
using View;

namespace Manager
{
    public class PoiManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPoiPrefab;
        [SerializeField] private AutoPoiView autoView;

        [Inject] private IPoiPresenter _poiPresenter;
        [Inject] DiContainer _diContainer;

        private GameObject _playerPoi;

        // Start is called before the first frame update
        void Start()
        {
            _poiPresenter.ObservableTeared
                .Subscribe(poiType => Create(poiType))
                .AddTo(this);

            foreach (PoiType type in Enum.GetValues(typeof(PoiType)))
            {
                Create(type);
            }
        }

        /// <summary>
        /// Poiを生成する関数
        /// </summary>
        /// <param name="poiType"></param>
        void Create(PoiType poiType)
        {
            var poi = _diContainer.InstantiatePrefab(playerPoiPrefab);

            if (poiType == PoiType.Player)
            {
                _playerPoi = poi;
            }

            // TODO: Add multiplay later 
        }
    }
}