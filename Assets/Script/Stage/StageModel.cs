using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using View;
using Data;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace Model
{
    public class StageModel
    {
        private Subject<StageData> _initSubject = new();
        private Subject<Unit> _clearSubject = new();
        private Subject<(FishType, int)> _remainConditionSubject = new();

        private const string StageDataKey = "FishStageData";
        private StageData _curData;
        private GameConfig _gameConfig;

        public IObservable<Unit> ObservableCleared => _clearSubject;
        public IObservable<(FishType, int)> ObservableOnChangedConditionsRemain => _remainConditionSubject;

        public StageModel(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }
        
        public StageData InitStageData()
        {
            //After that, when interworking with the LAMP server is completed, game data should be acquired
            //var data = LoadStageData();
            var data = CreateStageData();
            SaveStageData(data);
            
            _curData = data;
            return data;
        }

        public void ResetData()
        {
            PlayerPrefs.DeleteKey(StageDataKey);
        }

        public void Gotcha(FishType fishType)
        {
            //残りの数を減らす
            _curData.AppearFishInfos[fishType]--;
            
            if (_curData.GotchaProgress.ContainsKey(fishType))
            {
                //もう捕まえてたら
                _curData.GotchaProgress[fishType]++;
            }
            else
            {
                //このステージ初GETだったら
                _curData.GotchaProgress.Add(fishType, 1);
            }
            
            //クリア条件の魚だったら
            if (_curData.IsConditionsFish(fishType))
            {
                UpdateRemainCondition(fishType);
                CheckClear();
            }
            
            SaveStageData(_curData);
        }

        public void UpdateRemainCondition(FishType fishType)
        {
            if (_curData.GotchaProgress.ContainsKey(fishType))
            {
                _remainConditionSubject.OnNext((fishType, _curData.ClearConditions[fishType] - _curData.GotchaProgress[fishType]));
            }
        }

        void CheckClear()
        {
            foreach (var KeyValue in _curData.ClearConditions)
            {
                var fishType = KeyValue.Key;
                if(!_curData.HasClearedCondition(fishType))
                {
                    //まだクリアできてない条件がある
                    return;
                }
            }
            
            StageClear();
        }

        void StageClear()
        {
            _clearSubject.OnNext(Unit.Default);
        }
        
        void SaveStageData(StageData stageData)
        {
            string jsonData = JsonConvert.SerializeObject(stageData);
            PlayerPrefs.SetString(StageDataKey, jsonData);
            PlayerPrefs.Save();
        }

        StageData LoadStageData()
        {
            if (PlayerPrefs.HasKey(StageDataKey))
            {
                string jsonData = PlayerPrefs.GetString(StageDataKey);
                var stageData = JsonConvert.DeserializeObject<StageData>(jsonData);
                return stageData;
            }
            return null; // データがない場合はnullを返す
        }

        StageData CreateStageData()
        {
            var clearConditions = new Dictionary<FishType, int>();

            var easyFishes = FishDataManager.Instance.GetFishes(Difficulty.Easy);
            var normalFishes = FishDataManager.Instance.GetFishes(Difficulty.Normal);
            var hardFishes = FishDataManager.Instance.GetFishes(Difficulty.Hard);

            // 配列をシャッフル
            ShuffleArray(easyFishes);
            ShuffleArray(normalFishes);
            ShuffleArray(hardFishes);

            // 출현할 물고기를 결정 및 출현 정보와 클리어 조건 한 번에 처리
            Dictionary<FishType, int> appearFishInfos = new();

            void AddFishData(FishType[] fishes, int fishInStage, int fishInClearCondition, int numberOfFish)
            {
                var stageFishes = fishes.Take(fishInStage).ToList();

                foreach (var fish in stageFishes)
                {
                    appearFishInfos.Add(fish, numberOfFish);
                }

                foreach (var fish in stageFishes.Take(fishInClearCondition))
                {
                    clearConditions.Add(fish, numberOfFish);
                }
            }

            AddFishData(easyFishes, _gameConfig.EasyFishTypesInStage, _gameConfig.NumberOfEasyClearConditionTypes, _gameConfig.NumberOfEasyFish);
            AddFishData(normalFishes, _gameConfig.NormalFishTypesInStage, _gameConfig.NumberOfNormalClearConditionTypes, _gameConfig.NumberOfNormalFish);
            AddFishData(hardFishes, _gameConfig.HardFishTypesInStage, _gameConfig.NumberOfHardClearConditionTypes, _gameConfig.NumberOfHardFish);

            return new StageData(appearFishInfos, clearConditions);
        }


        void ShuffleArray<T>(T[] array)
        {
            Random random = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}