using UnityEngine;
using TMPro;
using System.Collections;

public class Pulpit : MonoBehaviour
{
    public TMP_Text countdownText;
    private float destroyTime;
    private float spawnTime;

    private float timer;
    private bool isActive = true;

    void Start()
    {
        destroyTime = 5.0f;
        timer = destroyTime;
        StartCoroutine(InitializePulpit());
    }

    IEnumerator InitializePulpit()
    {
        while (DataManager.Instance == null || DataManager.Instance.GetData() == null || DataManager.Instance.GetData().pulpit_data == null)
        {
            Debug.Log("Pulpit: Waiting for GameData...");
            yield return null;
        }

        GameData data = DataManager.Instance.GetData();

        if (data != null)
        {
            destroyTime = Random.Range(data.pulpit_data.min_pulpit_destroy_time, data.pulpit_data.max_pulpit_destroy_time);
            spawnTime = data.pulpit_data.pulpit_spawn_time;
            timer = destroyTime;
            
        }
        else
        {
            Debug.LogError("Pulpit: GameData is not set.");
        }
    }

    void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;
            countdownText.text = timer.ToString();
            Debug.Log("Pulpit Timer: " + timer);

            if (timer <= 0)
            {
                Debug.Log("Pulpit destroyed at time: " + timer);
                DestroyPulpit();
            }
        }
    }

    void DestroyPulpit()
    {
        if (isActive)
        {
            isActive = false;
            GameManager.Instance.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
