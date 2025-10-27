using System;
using UnityEngine;

[Serializable]
public class ScoreData : MonoBehaviour
{
    public int score;
    public long timestamp;

    public ScoreData()
    {

    }

    public ScoreData(int score, long timestamp)
    {
        this.score = score;
        this.timestamp = timestamp;
    }

    public DateTime GetDataTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        // 타임 스탬프를 현재 디바이스에 설정된 지역의 시간에 맞추어 리턴
    }

    public string GetDateString()
    {
        return GetDataTime().ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static ScoreData FromJson(string json)
    {
        return JsonUtility.FromJson<ScoreData>(json);
    }
}