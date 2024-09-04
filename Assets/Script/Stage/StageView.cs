using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Data;
using UniRx;

namespace View
{
    public class StageView : MonoBehaviour
    {
        [Inject] IStagePresenter _stagePresenter;
        [Inject] private DiContainer _diContainer;

        private GameObject _curStageFishes;
        
        // Start is called before the first frame update
        void Start()
        {
            _stagePresenter.ObservableAppearFishes
                .Subscribe(fishInfos => CreateFishes(fishInfos))
                .AddTo(this);

            _stagePresenter.ObservableCleared
                .Subscribe(_ => StageCleared())
                .AddTo(this);

            Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize()
        {
            if (_curStageFishes) Destroy(_curStageFishes);

            _stagePresenter.InitStage();
        }

        void CreateFishes(Dictionary<FishType, int> fishInfos)
        { 
            _curStageFishes = new GameObject();
            _curStageFishes.transform.position = transform.position;

            foreach (var fishInfo in fishInfos)
            {
                var prefab = FishDataManager.Instance.GetPrefab(fishInfo.Key);
                for (int fishcount = 0; fishcount < fishInfo.Value; fishcount++)
                {
                    var fish = _diContainer.InstantiatePrefab(prefab);
                
                    var randomPos = PoolManager.Instance.GetRandomTargetPoint();
                    fish.transform.position = new Vector3(randomPos.x, fish.transform.position.y, randomPos.z);
                    fish.transform.SetParent(_curStageFishes.transform);
                }
            }
        }

        void StageCleared()
        {
            Initialize();
        }
    }
}