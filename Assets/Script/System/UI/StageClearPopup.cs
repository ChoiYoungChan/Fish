using System;
using UniRx;
using Zenject;

public class StageClearPopup : BasePopupView
{
    [Inject] private IStagePresenter _stagePresenter;

    public override void Init()
    {
        _stagePresenter.ObservableCleared
            .Subscribe(_ => this.Show())
            .AddTo(this);
    }

    public override void Show()
    {
        base.Show();
    }

    public void Close()
    {
        Hide(0.3f);
    }
}