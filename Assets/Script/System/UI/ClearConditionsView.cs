using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Data;
using Zenject;

namespace View
{
    public class ClearConditionsView : MonoBehaviour
    {
        [SerializeField] private GameObject signPrefab;
        [SerializeField] private Transform signBase;
        [Inject] private IStagePresenter _stagePresenter;
        [Inject] private DiContainer _diContainer;
        private List<GameObject> _signs = new();
        
        // Start is called before the first frame update
        void Awake()
        {
            _stagePresenter.ObservableClearConditions
                .Subscribe(conditions => InitConditions(conditions))
                .AddTo(this);
        }

        private void InitConditions(Dictionary<FishType, int> conditions)
        {
            if (_signs.Count > 0)
            {
                for (int count = _signs.Count - 1; count >= 0; count--)
                {
                    Destroy(_signs[count]);
                }
                _signs.Clear();
            }

            foreach (var condition in conditions)
            {
                var sign = _diContainer.InstantiatePrefab(signPrefab);
                sign.transform.SetParent(signBase);
                sign.GetComponent<ConditionSign>().Init(condition.Key, condition.Value);
                _signs.Add(sign);
            }
        }
    }
}