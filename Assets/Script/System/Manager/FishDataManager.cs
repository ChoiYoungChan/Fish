using UnityEngine;
using Data;

public class FishDataManager : SingletonClass<FishDataManager>
{
    public DifficultyFishData[] difficultyFishConfigs;

    /// <summary>
    /// Get a list of FishType corresponding to the difficulty level
    /// </summary>
    /// <param name="difficulty">difficulty</param>
    /// <returns></returns>
    public FishType[] GetFishes(Difficulty difficulty)
    {
        foreach (var config in difficultyFishConfigs)
        {
            if (config.difficulty == difficulty)
            {
                return config.GetFishes();
            }
        }
#if DEBUG_MODE
        Debug.LogWarning($"Fishes not found for Difficulty: {difficulty}");
# endif
        return new FishType[0];
    }

    /// <summary>
    /// Get GameObject for FishType
    /// </summary>
    /// <param name="fishType">fish type</param>
    /// <returns></returns>
    public GameObject GetPrefab(FishType fishType)
    {
        foreach (var config in difficultyFishConfigs)
        {
            var prefab = config.GetPrefab(fishType);
            if (prefab != null)
            {
                return prefab;
            }
        }

#if DEBUG_MODE
        Debug.LogWarning($"Prefab not found for FishType: {fishType}");
#endif
        return null;
    }
}