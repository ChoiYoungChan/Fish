using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        // 最適化(Optimizatio)のために使わないStart関数はなくしました
        
        public void SetSeconds(float sec)
        {
            var time = (int)sec;
            int minutes = time / 60;  // 全秒数を60で割って分を得る
            int remainingSeconds = time % 60;  // 60で割った余りが残りの秒数

            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, remainingSeconds);
        }
    }
}