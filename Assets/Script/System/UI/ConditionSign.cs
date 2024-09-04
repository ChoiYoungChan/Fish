using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Data;

public class ConditionSign : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private RectTransform checkMarkRect;
    [SerializeField] private Image icon;
    [SerializeField] private List<Sprite> signIconSprites;
    [Inject] private IStagePresenter _stagePresenter;

    private FishType _fishType;
    private RectTransform _rectTransform;

    private void Awake()
    {
        checkMarkRect.localScale = Vector3.zero;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(FishType type, int remain)
    {
        _fishType = type;
        icon.sprite = signIconSprites[(int)type];
        SetSubscribe();
        ChangeCount(remain);
    }

    void SetSubscribe()
    {
        _stagePresenter.ObservableOnChangedConditionsRemain
            .Where(tuple => tuple.Item1 == _fishType)
            .Subscribe(tuple => ChangeCount(tuple.Item2))
            .AddTo(this);
    }

    void ChangeCount(int remain)
    {
        if (remain == 0)
        {
            countText.text = "";
            checkMarkRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            countText.text = remain.ToString();
            _rectTransform.DOScale(_rectTransform.localScale * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
    }
}