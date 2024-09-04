using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : SingletonClass<TimeManager>
{
    private Dictionary<string, float> _timeDict = new();
    private float _defaultTimeScale;

    void Start()
    {
        _defaultTimeScale = Time.timeScale;
    }

    void Update()
    {
        AddDeltaTime();
    }

    void AddDeltaTime()
    {
        if (_timeDict.Count == 0) return;

        foreach (var key in _timeDict.Keys.ToList())
        {
            float value = _timeDict[key];
            _timeDict[key] = value + Time.unscaledDeltaTime;
        }
    }

    public void SetNewTimer(string timerKey)
    {
        if (_timeDict.ContainsKey(timerKey)) return;
        _timeDict.Add(timerKey, 0);
    }

    public float GetTime(string timerKey)
    {
        return _timeDict[timerKey];
    }

    public void ResetTimer(string timerKey)
    {
        _timeDict[timerKey] = 0;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Time.timeScale = _defaultTimeScale;
    }

    public bool IsPausing()
    {
        return Time.timeScale == 0;
    }
}
