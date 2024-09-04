using System;
using System.Collections.Generic;
using Data;
using UniRx;

public interface IStagePresenter
{
    void InitStage();
    void Gotcha(FishType fishType);
    IObservable<Dictionary<FishType, int>> ObservableAppearFishes { get; }
    IObservable<Dictionary<FishType, int>> ObservableClearConditions { get; }
    IObservable<(FishType, int)> ObservableOnChangedConditionsRemain { get; }
    IObservable<Dictionary<FishType, int>> ObservableGotchaProgress { get; }
    IObservable<Unit> ObservableCleared { get; }
}
