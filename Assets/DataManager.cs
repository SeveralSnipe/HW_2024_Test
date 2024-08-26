using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public event Action OnDataLoaded;

    private GameData gameData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetData(GameData data)
    {
        gameData = data;
        OnDataLoaded?.Invoke();  // Trigger the event if there are subscribers
        Debug.Log("Game data set successfully.");
    }

    public GameData GetData()
    {
        return gameData;
    }
}
