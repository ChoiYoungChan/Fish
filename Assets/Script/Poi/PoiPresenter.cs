using System;
using View;
using UnityEngine;
using UniRx;

public interface IPoiPresenter
{
    void Move(PoiType poiType, Vector3 targetPos);
    void Up(PoiType poiType);
    void Teared(PoiType poiType);

    IObservable<(PoiType poiType, Vector3 targetPos)> ObservableMove { get; }
    IObservable<PoiType> ObservableUp { get; }
    IObservable<PoiType> ObservableTeared { get; }
}

public class PoiPresenter : IPoiPresenter
{
    public IObservable<(PoiType poiType, Vector3 targetPos)> ObservableMove => _moveSubject;
    public IObservable<PoiType> ObservableUp => _upSubject;
    public IObservable<PoiType> ObservableTeared => _tearedSubject;

    private Subject<(PoiType poiType, Vector3 targetPos)> _moveSubject = new();
    private Subject<PoiType> _upSubject = new();
    private Subject<PoiType> _tearedSubject = new();

    /// <summary>
    /// Poiを動かす時
    /// </summary>
    /// <param name="poiType"></param>
    /// <param name="targetPos"></param>
    public void Move(PoiType poiType, Vector3 targetPos)
    {
        _moveSubject.OnNext((poiType, targetPos));
    }

    /// <summary>
    /// Poiを持ち上げる時
    /// </summary>
    /// <param name="poiType"></param>
    public void Up(PoiType poiType)
    {
        _upSubject.OnNext(poiType);
    }

    /// <summary>
    /// Poiの紙が破れた時
    /// </summary>
    /// <param name="poiType"></param>
    public void Teared(PoiType poiType)
    {
        _tearedSubject.OnNext(poiType);
    }
}