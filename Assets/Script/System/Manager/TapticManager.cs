using System;
using System.Collections;
using UnityEngine;
using UniRx;

namespace Manager
{
    public enum NotificationFeedback
    {
        Success,
        Warning,
        Error
    }

    public enum ImpactFeedback
    {
        Light,
        Midium,
        Heavy
    }

    public class TapticManager : SingletonClass<TapticManager>
    {
        bool hasStartLoop;

        public Subject<bool> OnSwitched = new Subject<bool>();

        public void Switch()
        {
            var isOn = IsOn();
            PlayerPrefs.SetInt("HapticIsOn", Convert.ToInt32(isOn));
            OnSwitched.OnNext(IsOn());
        }

        public bool IsOn()
        {
            return PlayerPrefs.GetInt("HapticIsOn", 0) == 0;
        }

        public void Notification(NotificationFeedback feedback)
        {
            if (IsOn())
            {
                _unityTapticNotification((int)feedback);
            }
        }

        public void Impact(ImpactFeedback feedback)
        {
            if (IsOn())
            {
                _unityTapticImpact((int)feedback);
            }
        }

        public void Selection()
        {
            if (IsOn())
            {
                _unityTapticSelection();
            }
        }

        public bool IsSupport()
        {
            return _unityTapticIsSupport();
        }

        public void TapButton()
        {
            _unityTapticImpact((int)ImpactFeedback.Light);
        }

        public void StartHapticCoroutine()
        {
            StartCoroutine("HapticCoroutine");
        }

        public void StopHapticCoroutine()
        {
            StopCoroutine("HapticCoroutine");
        }

        public IEnumerator HapticCoroutine()
        {
            while (true)
            {
                Impact(ImpactFeedback.Midium);
                yield return new WaitForSeconds(0.15f);
            }
        }

        #region DllImport

#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _unityTapticNotification(int type);
	[DllImport("__Internal")]
	private static extern void _unityTapticSelection();
	[DllImport("__Internal")]
	private static extern void _unityTapticImpact(int style);
	[DllImport("__Internal")]
	private static extern bool _unityTapticIsSupport();
#else
        private static void _unityTapticNotification(int type) { }

        private static void _unityTapticSelection() { }

        private static void _unityTapticImpact(int style) { }

        private static bool _unityTapticIsSupport() { return false; }
#endif

        #endregion // DllImport
    }
}
