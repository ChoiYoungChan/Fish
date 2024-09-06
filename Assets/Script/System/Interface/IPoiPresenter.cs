using System;
using UnityEngine;
using View;

public interface IPoiPresenter
{
    void Move(PoiType poiType, Vector3 targetPos);
    void Up(PoiType poiType);
    void Teared(PoiType poiType);

    IObservable<(PoiType poiType, Vector3 targetPos)> ObservableMove { get; }
    IObservable<PoiType> ObservableUp { get; }
    IObservable<PoiType> ObservableTeared { get; }
}
