using UnityEngine;
using Zenject;
using UniRx;

namespace View
{
    public class PowerUpButton : BaseButton
    {
        [SerializeField] private TimerDisplay timerDisplay;
        [Inject] private PoiPowerUpManager _powerUpManager;

        public override void Init()
        {
            base.Init();
            
            _powerUpManager.ObservableOnPowerUp
                .Subscribe(_ => timerDisplay.gameObject.SetActive(true))
                .AddTo(this);
            
            _powerUpManager.ObservableOnFinish
                .Subscribe(_ => timerDisplay.gameObject.SetActive(false))
                .AddTo(this);
            
            _powerUpManager.ObservableOnChangedTimer
                .Subscribe(sec => timerDisplay.SetSeconds(sec))
                .AddTo(this);

            timerDisplay.gameObject.SetActive(false);
        }
        
        public override void OnClick()
        {
            base.OnClick();
            _powerUpManager.PowerUp();
        }
    }
}