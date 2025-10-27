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
        // Ÿ�� �������� ���� ����̽��� ������ ������ �ð��� ���߾� ����
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