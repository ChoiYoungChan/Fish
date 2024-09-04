using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Manager;

public class VibeButton : BaseButton
{
    [SerializeField] Sprite activeSp;
    [SerializeField] Sprite inactiveSp;
    Image _image;

    private void Awake()
    {
        TapticManager.Instance.OnSwitched
            .Subscribe(enable => ChangeSprite(enable))
            .AddTo(this);

        ChangeSprite(TapticManager.Instance.IsOn());
    }

    public override void OnClick()
    {
        TapticManager.Instance.Switch();
    }

    void ChangeSprite(bool enable)
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _image.sprite = enable ? activeSp : inactiveSp;
    }
}