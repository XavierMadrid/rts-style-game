using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private GameObject enemyShipUnitPrefab = null;

    public float GameTime { get; private set; }
    private float waveTime;
    private int currentWaveNum;
    private int wavesReached;
    
    // hard coded wave option
    // wave number to respective wave duration in seconds (only changes to durations are explicitly written)
    // perhaps create new Wave object type to include enemy count and any other necessities
    private Dictionary<int, int> waveDurations = new() { {1, 120}, {2, 180}, {3, 150}, {7, 120}, {10, 90}, {15, 60}, {20, 30}};

    private EnemyWave[] waves =
    {
        Wave1, Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8, Wave9, Wave10,
        Wave11, Wave12, Wave13, Wave14, Wave15, Wave16, Wave17, Wave18, Wave19, Wave20 
    };
    
    private static readonly EnemyWave Wave1 = new (1, 1, 1);
    private static readonly EnemyWave Wave2 = new (2, 4, 3);
    private static readonly EnemyWave Wave3 = new (3, 2.5f, 4);
    private static readonly EnemyWave Wave4 = new (4, 2.5f, 6);
    private static readonly EnemyWave Wave5 = new (5, 2.5f, 8);
    private static readonly EnemyWave Wave6 = new(6, 2.5f, 10);
    private static readonly EnemyWave Wave7 = new(7, 2, 10);
    private static readonly EnemyWave Wave8 = new(8, 2, 15);
    private static readonly EnemyWave Wave9 = new(9, 2, 20);
    private static readonly EnemyWave Wave10 = new(10, 1.5f, 20);
    private static readonly EnemyWave Wave11 = new(11, 1.5f, 25);
    private static readonly EnemyWave Wave12 = new(12, 1.5f, 30);
    private static readonly EnemyWave Wave13 = new(13, 1.5f, 30);
    private static readonly EnemyWave Wave14 = new(14, 1.5f, 30);
    private static readonly EnemyWave Wave15 = new(15, 1, 30);
    private static readonly EnemyWave Wave16 = new(16, 1, 30);
    private static readonly EnemyWave Wave17 = new(17, 1, 30);
    private static readonly EnemyWave Wave18 = new(18, 1, 30);
    private static readonly EnemyWave Wave19 = new(19, 1, 30);
    private static readonly EnemyWave Wave20 = new(20, 0.5f, 40);

    public ObservableCollection<GameObject> EnemyShips = new();

    void Start()
    {
        waveTime = Wave1.PrepMinutes * 60;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) // DEV
        {
            Vector3 spawnPos = ManagerReferences.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, -1f);
            GameObject enemyShipClone = Instantiate(enemyShipUnitPrefab, spawnPos, Quaternion.identity);
            
            EnemyShips.Add(enemyShipClone);
        }

        GameTime += Time.deltaTime;
        waveTime -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.T)) waveTime = 0; // DEV

        if (waveTime <= 0)
        {
            PrepareEnemyWave(wavesReached < 20 ? waves[wavesReached] : Wave20);

            wavesReached++;
        }
    }

    private void PrepareEnemyWave(EnemyWave wave)
    {
        waveTime = wave.PrepMinutes * 60;

        int randAngle = Random.Range(0, 360);
        int radius = Random.Range(525, 550);
        int randXSign = Random.Range(1, 3);
        int randYSign = Random.Range(1, 3);
        Vector3 spawnCircleCenter = new Vector2(Mathf.Pow(-1, randXSign) * radius * Mathf.Cos(randAngle), 
            Mathf.Pow(-1, randYSign) * radius * Mathf.Sin(randAngle));
        
        SpawnEnemyWave(wave, spawnCircleCenter);
    }

    private void SpawnEnemyWave(EnemyWave wave, Vector2 spawnPos)
    {
        for (int i = 0; i < wave.EnemyCount; i++) // probably make specific wave setups in the future
        {
            float randDist = Random.Range(0, wave.EnemyCount); // larger circle the more enemies there are
            Vector3 randPos = randDist * Random.insideUnitCircle + spawnPos;
            randPos = new Vector3(randPos.x, randPos.y, -2);
            
            GameObject enemyClone = Instantiate(enemyShipUnitPrefab, randPos, Quaternion.identity);
            EnemyShips.Add(enemyClone);
        }
    }
}

public class EnemyWave
{
    public int WaveNumber { get; private set; }
    public float PrepMinutes { get; private set; }
    public int EnemyCount { get; private set; }
    
    public EnemyWave(int waveNumber, float prepMinutes, int enemyCount)
    {
        WaveNumber = waveNumber;
        PrepMinutes = prepMinutes;
        EnemyCount = enemyCount;
    }
}
