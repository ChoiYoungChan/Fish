using System;
using System.Collections.Generic;
using Data;
using Model;
using UniRx;
using Zenject;

namespace Presenter
{
    public class StagePresenter : IStagePresenter
    {
        public IObservable<Dictionary<FishType, int>> ObservableAppearFishes => _appearSubject;
        public IObservable<Dictionary<FishType, int>> ObservableClearConditions => _conditionsSubject;
        public IObservable<(FishType, int)> ObservableOnChangedConditionsRemain => _changedRemainSubject;
        public IObservable<Dictionary<FishType, int>> ObservableGotchaProgress => _progressSubject;
        public IObservable<Unit> ObservableCleared => _clearedSubject;
        
        private StageModel _model;
        private Subject<Dictionary<FishType, int>> _appearSubject = new();
        private Subject<Dictionary<FishType, int>> _conditionsSubject = new();
        private Subject<(FishType, int)> _changedRemainSubject = new();
        private Subject<Dictionary<FishType, int>> _progressSubject = new();
        private Subject<Unit> _clearedSubject = new();

        private List<IDisposable> _disposables = new();
        
        [Inject] GameConfig _gameConfig;

        public void InitStage()
        {
            if (_model != null)
            {
                _model.ResetData();
                _model = null;

                for (int count = 0; count < _disposables.Count; count++)
                {
                    _disposables[count].Dispose();
                }
                _disposables.Clear();
            }

            _model = new StageModel(_gameConfig);
            var disposable1 = _model.ObservableCleared.Subscribe(_ => _clearedSubject.OnNext(Unit.Default));
            _disposables.Add(disposable1);

            var disposable2 =
                _model.ObservableOnChangedConditionsRemain.Subscribe(tuple => _changedRemainSubject.OnNext(tuple));
            _disposables.Add(disposable2);
            
            var stageData = _model.InitStageData();

            UpdateAppearFishes(stageData.AppearFishInfos);
            UpdateClearConditions(stageData.ClearConditions);
            UpdateGotchaProgress(stageData.GotchaProgress);

            foreach (var condition in stageData.ClearConditions)
            {
                _model.UpdateRemainCondition(condition.Key);
            }
        }

        public void Gotcha(FishType fishType)
        {
            _model.Gotcha(fishType);
        }

        void UpdateAppearFishes(Dictionary<FishType, int> infos)
        {
            _appearSubject.OnNext(infos);
        }

        void UpdateClearConditions(Dictionary<FishType, int> conditions)
        {
            _conditionsSubject.OnNext(conditions);
        }

        void UpdateGotchaProgress(Dictionary<FishType, int> progress)
        {
            _progressSubject.OnNext(progress);
        }
    }
}