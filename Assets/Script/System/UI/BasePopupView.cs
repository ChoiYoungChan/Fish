using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BasePopupView : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] RectTransform popupRectTrm;

    private void Start()
    {
        Init();
        Hide();
    }

    /// <summary>
    /// 背景を見せる
    /// </summary>
    private void ShowBG()
    {
        bgImage.gameObject.SetActive(true);
        bgImage.DOFade(0.92f, 0.3f).OnComplete(() =>
        {
            popupRectTrm.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        });
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    public virtual void Init()
    {

    }

    /// <summary>
    /// Show処理
    /// </summary>
    public virtual void Show()
    {
        ShowBG();
    }

    /// <summary>
    /// Hide処理
    /// </summary>
    /// <param name="time"></param>
    public virtual void Hide(float time = 0f)
    {
        popupRectTrm.DOScale(Vector3.zero, time).SetEase(Ease.InBack);
        bgImage.DOColor(new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0), time).OnComplete(() =>
        {
            bgImage.gameObject.SetActive(false);
        });
    }
}