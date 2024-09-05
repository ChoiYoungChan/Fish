using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "NewFishConfig", menuName = "Fish Config")]
public class DifficultyFishData : ScriptableObject
{
    public Difficulty difficulty;
    public FishData[] fishDataArray;

    /// <summary>
    /// FishTypeに対応するGameObjectを返す関数
    /// </summary>
    /// <param name="fishType">魚タイプ</param>
    /// <returns></returns>
    public GameObject GetPrefab(FishType fishType)
    {
        foreach (var fishData in fishDataArray)
        {
            if (fishData.fishType == fishType)
            {
                return fishData.fishPrefab;
            }
        }

#if DEBUG_MODE
        Debug.LogWarning($"Prefab not found for FishType: {fishType}");
#endif
        return null;
    }

    /// <summary>
    /// 難易度に対応するFishTypeのリストを返す関数
    /// </summary>
    /// <returns></returns>
    public FishType[] GetFishes()
    {
        FishType[] fishTypes = new FishType[fishDataArray.Length];
        for (int fishcount = 0; fishcount < fishDataArray.Length; fishcount++)
        {
            fishTypes[fishcount] = fishDataArray[fishcount].fishType;
        }
        return fishTypes;
    }
}