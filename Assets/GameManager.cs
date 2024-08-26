using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float spawnOffset = 10.0f;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public GameObject gameOverPanel;
    public GameObject pulpitPrefab;
    public Transform[] spawnPoints;

    private int score = 0;
    private int activePulpits = 1;

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

    void Start()
    {
        gameOverPanel.SetActive(false);
        JsonDataFetcher dataFetcher = FindObjectOfType<JsonDataFetcher>();
        if (dataFetcher != null)
        {
            dataFetcher.onDataLoaded += InitializeGameAfterDataLoad;
        }
        else
        {
            Debug.LogError("JsonDataFetcher not found in the scene.");
        }
    }

    void InitializeGameAfterDataLoad()
{
    Debug.Log("GameData loaded successfully. Initializing game...");
    GameObject initialPulpit = FindObjectOfType<Pulpit>()?.gameObject;
    if (initialPulpit != null)
    {
        SpawnPulpitsAround(initialPulpit);
    }
    else
    {
        Debug.LogError("No initial pulpit found in the scene.");
    }
}


    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        Debug.Log(score);
    }

IEnumerator SpawnPulpitsAfterDelay(GameObject destroyedPulpit)
{
    while (DataManager.Instance == null || DataManager.Instance.GetData() == null || DataManager.Instance.GetData().pulpit_data == null)
    {
        Debug.Log("GameManager: Waiting for data to load...");
        yield return null;
    }

    GameData data = DataManager.Instance.GetData();
    Debug.Log("GameManager: Data loaded, spawning pulpits...");

    yield return new WaitForSeconds(data.pulpit_data.pulpit_spawn_time);
    SpawnPulpitsAround(destroyedPulpit);
    activePulpits++;
}


    void SpawnPulpitsAround(GameObject currentPulpit)
{
    Vector3 currentPosition = currentPulpit.transform.position;
    Vector3[] spawnPositions = new Vector3[]
    {
        currentPosition + Vector3.forward * spawnOffset,
        currentPosition + Vector3.back * spawnOffset,
        currentPosition + Vector3.left * spawnOffset,
        currentPosition + Vector3.right * spawnOffset
    };

    int randomIndex = Random.Range(0, spawnPositions.Length);
    Vector3 randomSpawnPosition = spawnPositions[randomIndex];

    StartCoroutine(SpawnWithDelay(randomSpawnPosition));
}

IEnumerator SpawnWithDelay(Vector3 position)
{
    while (DataManager.Instance == null || DataManager.Instance.GetData() == null || DataManager.Instance.GetData().pulpit_data == null)
    {
        yield return null;
    }

    GameData data = DataManager.Instance.GetData();
    yield return new WaitForSeconds(data.pulpit_data.pulpit_spawn_time);
    GameObject newPulpit = Instantiate(pulpitPrefab, position, Quaternion.identity);
    activePulpits++;
    SpawnPulpitsAround(newPulpit);
}


    public void GameOver()
    {
        finalScoreText.text = "Final Score: " + score.ToString();
        gameOverPanel.SetActive(true);
        Debug.Log("Opened final score");
        Time.timeScale = 0;
    }
}
