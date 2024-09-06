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

    /// <summary>
    /// Awake時に初期化
    /// </summary>
    private void Awake()
    {
        checkMarkRect.localScale = Vector3.zero;
        _rectTransform = GetComponent<RectTransform>();

        // モバイルとPCの際、別scaleを適用
#if UNITY_EDITOR
        _rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
#elif UNITY_ANDROID
        _rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
#else
        _rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
#endif

    }

    /// <summary>
    /// 捕まった魚数を反映する
    /// </summary>
    /// <param name="remain"></param>
    private void ChangeCount(int remain)
    {
        if (remain == 0)
        {
            countText.text = "";
            checkMarkRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetLink(gameObject);
        }
        else
        {
            countText.text = remain.ToString();
            _rectTransform.DOScale(_rectTransform.localScale * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }

    private void SetSubscribe()
    {
        _stagePresenter.ObservableOnChangedConditionsRemain
            .Where(tuple => tuple.Item1 == _fishType)
            .Subscribe(tuple => ChangeCount(tuple.Item2))
            .AddTo(this);
    }

    /// <summary>
    /// ClearConditionsViewから呼び出す初期化処理
    /// </summary>
    /// <param name="type"></param>
    /// <param name="remain"></param>
    public void Init(FishType type, int remain)
    {
        _fishType = type;
        icon.sprite = signIconSprites[(int)type];
        SetSubscribe();
        ChangeCount(remain);
    }
}