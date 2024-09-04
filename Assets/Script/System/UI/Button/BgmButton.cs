using NBLib;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace NBLib
{
    public class BgmButton : BaseButton
    {
        [SerializeField] Sprite activeSp;
        [SerializeField] Sprite inactiveSp;
        Image _image;

        private void Awake()
        {
            SoundManager.Instance.OnSwitchedBgm
                .Subscribe(enable => ChangeSprite(enable))
                .AddTo(this);

            ChangeSprite(SoundManager.Instance.IsOnBgm());
        }

        public override void OnClick()
        {
            SoundManager.Instance.SwitchBgm();
        }

        void ChangeSprite(bool enable)
        {
            if (_image == null) _image = GetComponent<Image>();

            _image.sprite = enable ? activeSp : inactiveSp;
        }
    }
}