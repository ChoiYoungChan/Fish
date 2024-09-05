using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class FishData
    {
        public FishType fishType;
        public GameObject fishPrefab;
    }
    
    [System.Serializable]
    public class StageData
    {
        public Dictionary<FishType, int> AppearFishInfos;
        public Dictionary<FishType, int> ClearConditions;
        public Dictionary<FishType, int> GotchaProgress;

        public StageData(Dictionary<FishType, int> appearFishInfos, Dictionary<FishType, int> clearConditions)
        {
            AppearFishInfos = appearFishInfos;
            ClearConditions = clearConditions;
            GotchaProgress = new Dictionary<FishType, int>();
        }

        /// <summary>
        /// 捕まえたのか捕まえて無いのかを判断
        /// </summary>
        /// <param name="fishType"></param>
        /// <returns></returns>
        public bool HasClearedCondition(FishType fishType)
        {
            //クリア
            if (!IsConditionsFish(fishType))
            {
                Debug.LogError("## クリア条件ではない魚です");
            }
            
            //まだ捕まえてなかったらfalse
            if (!GotchaProgress.ContainsKey(fishType)) return false;

            //捕まえた数がクリア条件以上かどうか
            return GotchaProgress[fishType] >= ClearConditions[fishType];
        }

        public bool IsConditionsFish(FishType fishType)
        {
            return ClearConditions.ContainsKey(fishType);
        }
    }
}