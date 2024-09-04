using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace NBLib
{
    public class SeButton : BaseButton
    {
        [SerializeField] Sprite activeSp;
        [SerializeField] Sprite inactiveSp;
        Image _image;

        private void Awake()
        {
            SoundManager.Instance.OnSwitchedSe
                .Subscribe(enable => ChangeSprite(enable))
                .AddTo(this);

            ChangeSprite(SoundManager.Instance.IsOnSe());
        }

        public override void OnClick()
        {
            SoundManager.Instance.SwitchSe();
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
}
