using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private Transform celBodyParentHolder = null;
    
    private static readonly SubBody YELLOW_STAR = new("Yellow Star", 40, 100, 10.9f, null, 0);    
    private static readonly SubBody ORANGE_STAR = new("Orange Star", 10, 101, 22.9f, null, 1);    
    private static readonly SubBody RED_STAR = new("Red Star", 50, 102, 7.2f, null, 2);

    private static readonly SubBody METAL_PLANET = new("Metal Planet", 50, 103, 6.3f, null, 3);
    private static readonly SubBody FOREST_PLANET = new("Metal Planet", 50, 104, 6.3f, null, 4);

    private static readonly CelBody PLANET = new("Planet", 70, 0, 6.3f,
        new [] {METAL_PLANET, FOREST_PLANET});
    private static readonly CelBody STAR = new("Star", 30, 1, 10.9f,
        new [] {YELLOW_STAR, ORANGE_STAR, RED_STAR});

    private static readonly CelBody[] celBodies = { PLANET, STAR };

    [FormerlySerializedAs("celestialBodies")] [SerializeField] private GameObject[] celBodyPrefabs = new GameObject[5];

    int[] subBodyIDS_DEV = new int[5]; // DEV ONLY

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            RandomlySpawnCelBodies(celBodies, i);
        }
        PrintCelBodyTypeCount_DEV(); // DEV ONLY
    }

    private void RandomlySpawnCelBodies(CelBody[] celBodies, int index)
    {
        // essentially randomly chooses to spawn a planet or star based on their randomSpawningWeight value
        CelBody celBodyChoice = WeightedRandom.ChooseCelBody(celBodies);

        if (celBodyChoice.SubBodies is not null)
        {
            int count = celBodyChoice.SubBodies.Length;
            CelBody[] subBodies = new CelBody[count];
            for (int i = 0; i < count; i++)
            {
                subBodies[i] = celBodyChoice.SubBodies[i];
            }
            for (int i = 0; i < count; i++) RandomlySpawnCelBodies(subBodies, i);
        }
        else
        {
            float xPos = 0, yPos = 0, randRotation = 0;
            for (int i = 0; i < 3; i++)
            {
                xPos = Random.Range(-1000f, 1000f);
                yPos = Random.Range(-550f, 550f);

                if (xPos is >= -40 and <= 40 && yPos is <= 40 and >= -40) return; // prevent spawning planets or stars near the player spawn
                
                Collider2D obstructingCol = Physics2D.OverlapCircle(new Vector2(xPos, yPos), celBodyChoice.Radius, LayerMask.GetMask("CelBodies"));
                if (obstructingCol is null) break;

                if (i == 2) return;
            }
            
            randRotation = Random.Range(0f, 360f);

            if (celBodyChoice.ID - 100 < 0)
            {
                throw new ArgumentOutOfRangeException("An object without any sub bodies tried to spawn. " +
                                                      "Without any prefab with its ID, no object can be spawned. " +
                                                      $"Object Name: {celBodyChoice.Name}, Object ID: {celBodyChoice.ID}");
            }
            subBodyIDS_DEV[celBodyChoice.ID - 100]++; // DEV ONLY
            // please put into an array
            
            GameObject celBodyClone = Instantiate(celBodyPrefabs[celBodyChoice.ID - 100], new Vector3(xPos, yPos),
            Quaternion.Euler(0, 0, randRotation), celBodyParentHolder);
        }
    }
    
    private void PrintCelBodyTypeCount_DEV() // DEV ONLY
    {
        foreach (var id in subBodyIDS_DEV)
            Debug.Log(id);
    }
}

public class CelBody // from celestial body
{
    public string Name { get; protected set; }
    public float SpawningWeight { get; protected set; }
    public int ID { get; protected set; }
    public float Radius { get; protected set; }
    public SubBody[] SubBodies { get; protected set; }
    
    public CelBody(string celBodyName, float celBodyRandomSpawningWeight, int iD, float radius = 6.3f, SubBody[] subBodies = null)
    {
        Name = celBodyName;
        SpawningWeight = celBodyRandomSpawningWeight;
        ID = Mathf.Clamp(iD, 0, 99);
        Radius = radius;
        SubBodies = subBodies;
    }
}

public class SubBody : CelBody
{
    public int PrefabID { get; private set; }
    
    public SubBody(string celBodyName, float celBodyRandomSpawningWeight, int iD, float radius, SubBody[] subBodies, int prefabID)
        : base(celBodyName, celBodyRandomSpawningWeight, iD, radius, subBodies)
    {
        Name = celBodyName;
        SpawningWeight = celBodyRandomSpawningWeight;
        ID = Mathf.Clamp(iD, 100, 999);
        Radius = radius;
        SubBodies = subBodies;
        PrefabID = Mathf.Clamp(prefabID, 0, 99);
    }
}
