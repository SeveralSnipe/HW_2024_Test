using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private GameData gameData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetData(GameData data)
    {
        gameData = data;
        if (gameData != null && gameData.pulpit_data != null)
        {
            Debug.Log("DataManager: GameData and PulpitData are set.");
        }
        else
        {
            Debug.LogError("DataManager: GameData or PulpitData is null.");
        }
    }

    public GameData GetData()
    {
        return gameData;
    }
}
