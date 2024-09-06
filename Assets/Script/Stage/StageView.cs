using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Data;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace View
{
    public class StageView : MonoBehaviour
    {
        [Inject] IStagePresenter _stagePresenter;
        [Inject] private DiContainer _diContainer;

        private GameObject _curStageFishes;
        private CancellationTokenSource _cancellationTokenSource;

        // Start is called before the first frame update
        void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _stagePresenter.ObservableAppearFishes
                .Subscribe(fishInfos => CreateFishesAsync(fishInfos,
                                        this.GetCancellationTokenOnDestroy()).Forget()).AddTo(this);

            _stagePresenter.ObservableCleared
                .Subscribe(_ => StageCleared())
                .AddTo(this);
            
            Initialize();
        }

        /// <summary>
        /// UniTaskを利用する場合のInitialize
        /// </summary>
        private void Initialize()
        {
            if (_curStageFishes) Destroy(_curStageFishes);

            _stagePresenter.InitStage();
        }

        /// <summary>
        /// 魚の生成にUniTaskを使う事で初期起動時のフレームが落ちる問題を解決した。
        /// </summary>
        /// <param name="fishInfos">魚のタイプ</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        async UniTaskVoid CreateFishesAsync(Dictionary<FishType, int> fishInfos,
                                            CancellationToken cancellationToken)
        {
            _curStageFishes = new GameObject();
            _curStageFishes.transform.position = transform.position;

            foreach (var fishInfo in fishInfos)
            {
                var prefab = FishDataManager.Instance.GetPrefab(fishInfo.Key);
                for (int fishcount = 0; fishcount < fishInfo.Value; fishcount++)
                {
                    // キャンセル要請があれば作業中止
                    if (cancellationToken.IsCancellationRequested) return;
                    // Instantiate
                    var fish = _diContainer.InstantiatePrefab(prefab);
                    // ランダム座標に生成
                    var randomPos = PoolManager.Instance.GetRandomTargetPoint();
                    fish.transform.position =
                        new Vector3(randomPos.x, fish.transform.position.y, randomPos.z);

                    fish.transform.SetParent(_curStageFishes.transform);

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
            }
        }

        /// <summary>
        /// ステージをクリアした際に一度初期化
        /// </summary>
        private void StageCleared()
        {
            Initialize();
        }

        void OnDestroy()
        {
            // オブジェクトが破壊されたとき作業中止
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
