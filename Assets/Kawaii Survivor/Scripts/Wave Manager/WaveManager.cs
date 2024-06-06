using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour,IGameStateListner
{
    [Header("Elements")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;

    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();
    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }
    private void Start()
    {
        StartWave(currentWaveIndex);
    }
    private void Update()
    {
        if (!isTimerOn)
            return;

        if(timer < waveDuration)
        {
            ManageCurrentWave();

            string timerString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);
        }
        else
        {
            StartWaveTransition();
        }
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemies();
        currentWaveIndex++;

        if(currentWaveIndex >= waves.Length)
        {
            ui.UpdateWaveText("Stage Completed");
            ui.UpdateTimerText("");
        }
        else
        {
            GameManager.instance.WaveCompletedCallback();
        }
    }

    private void DefeatAllEnemies()
    {
        transform.Clear();
    }

    private void StartWave(int waveIndex)
    {
        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + waves.Length);

        localCounters.Clear();
        foreach(WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(1);

        timer = 0;
        isTimerOn = true;
    }
    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;
            
            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

            if(timeSinceSegmentStart /spawnDelay  > localCounters[i])
            {
                Instantiate(segment.prefab,GetSpawnPosition(),Quaternion.identity,transform);
                localCounters[i]++;
            }
        }
        timer += Time.deltaTime;
    }
    Vector2 offSetRadius;
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offSet = direction.normalized * Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offSet;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -8, 8);
        offSetRadius = offSet;
        return targetPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(player.transform.position, offSetRadius.magnitude);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        Debug.Log($"wave manager{gameState}");
    }
}
[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
}
