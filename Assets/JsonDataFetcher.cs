using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class JsonDataFetcher : MonoBehaviour
{
    private const string url = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    void Start()
    {
        StartCoroutine(FetchJsonData());
    }

    IEnumerator FetchJsonData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            ParseJsonData(json);
        }
    }

    void ParseJsonData(string json)
    {
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        DataManager.Instance.SetData(gameData);
    }
}


[System.Serializable]
public class GameData
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

[System.Serializable]
public class PlayerData
{
    public int speed;
}

[System.Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}
