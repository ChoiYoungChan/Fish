using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PoiPowerUpManager : ITickable  //Managerというなのただのタイマー
{
    public IObservable<Unit> ObservableOnPowerUp => _powerUpSubject;
    public IObservable<Unit> ObservableOnFinish => _finishSubject;
    public IObservable<float> ObservableOnChangedTimer => _changedTimerSubject;
    public int PoiFormIndex => _poiFormIndex;

    private float _timer;
    private int _poiFormIndex;
    private Subject<Unit> _powerUpSubject = new();
    private Subject<Unit> _finishSubject = new();
    private Subject<float> _changedTimerSubject = new();

    [Inject] private GameConfig _gameConfig;

    public void PowerUp(Action rewardCallBack = null)
    {
        _timer += 60f;
        _poiFormIndex = Math.Min(++_poiFormIndex, _gameConfig.PoiHPs.Length - 1);
        _powerUpSubject.OnNext(Unit.Default);
    }

    public void Tick()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime * Time.timeScale;

            if (_timer < 0f)
            {
                Finish();
            }

            _changedTimerSubject.OnNext(_timer);
        }
    }

    void Finish()
    {
        _timer = 0f;
        _poiFormIndex = 0;
        _finishSubject.OnNext(Unit.Default);
    }
}