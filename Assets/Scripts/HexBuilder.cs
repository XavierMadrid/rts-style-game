using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HexBuilder : MonoBehaviour
{
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private GameObject hexExample = null;
    
    private static readonly Color CenterHexColor = new(.729f, .820f, 1, 1);
    private static readonly Color GateHexColor = new(.3208f, .3208f, .3208f, .6471f);
    private static readonly Color PowerHexColor = new(0, 0.5764f, 1, 1);
    private static readonly Color WorkshopHexColor = new(.0218f, 0.3019f, 0, 1);
    private static readonly Color GoldmineHexColor = new(1, .733f, 0, 1);
    private static readonly Color StarCollectorHexColor = new(.71f, .286f, 0, 1);
    
    public static readonly HexTileType CENTER_HEX = new("Center Hex", CenterHexColor, 0);
    
    public static readonly HexTileType GATE_HEX = new("Connector Hex",  GateHexColor, 1, 
        new ResourcePrice[] {new(ResourceManager.METAL, 30)}, KeyCode.Alpha1);
    
    public static readonly HexTileType POWER_HEX = new("Power Hex", PowerHexColor, 2,
        new ResourcePrice[] {new(ResourceManager.METAL, 200), new(ResourceManager.ENERGY, 120)}, KeyCode.Alpha2);
    
    public static readonly HexTileType WORKSHOP_HEX = new("Workshop Hex", WorkshopHexColor, 3, 
        new ResourcePrice[] {new(ResourceManager.METAL, 180), new(ResourceManager.GREENERY, 60)},KeyCode.Alpha3);
    
    public static readonly HexTileType GOLDMINE_HEX = new("Golden Hex",  GoldmineHexColor, 4, 
        new ResourcePrice[] {new(ResourceManager.METAL, 300)}, KeyCode.Alpha4);
    
    public static readonly HexTileType STAR_COLLECTOR_HEX = new("Star Collector Hex",  StarCollectorHexColor, 5, 
        new ResourcePrice[] {new(ResourceManager.METAL, 200)}, KeyCode.Alpha5);

    [FormerlySerializedAs("hexPrefabs")] [FormerlySerializedAs("hexTypes")] [SerializeField] private GameObject[] hexTileTypePrefabs;
    
    private Color failedHexPlaceColor = new(1, 0, 0, .5f);
    
    private List<Hex> occupiedHexes = new();
    private GameObject centerHex;
    private GameObject hexPrefabSelected;
    private HexTileType hexTypeSelected;
    private Color hexColor;
    private ResourcePrice[] resourcePrices;
    private GameObject bluePrintHex;
    private SpriteRenderer bluePrintHexsr;
    private bool resetColor;
    private bool buildMode;
    private bool hexPlaceFailure;
    public bool BuildMode
    {
        get => buildMode;
        set
        {
            if (value && !buildMode)
            {
                OnBuildMode();
            }
            else if (!value && buildMode)
            {
                OnBuildModeOff();
            }
            else
            {
                resetColor = true;
            }
            buildMode = value;
        }
    }

    private WaitForSeconds redDuration = new WaitForSeconds(.10f);

    // Start is called before the first frame update
    void Start()
    {
        centerHex = Instantiate(hexTileTypePrefabs[CENTER_HEX.PrefabID], Vector3.zero, Quaternion.identity);
        occupiedHexes.Add(centerHex.transform.position.ToHex());
    }

    // Update is called once per frame
    void Update()
    {
        if (BuildMode)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition).SnapWorld();
            mousePos = new Vector3(mousePos.x, mousePos.y, -2);
            bluePrintHex.transform.position = mousePos;
            
            if (Input.GetMouseButtonDown(0))
            {
                Hex hexPos = mousePos.ToHex();

                if (IsHexOccupied(hexPos) || !HasNeighborHex(hexPos)) // check hex position viability
                {
                    hexPlaceFailure = true;
                    return;
                }
  
                int pricesMet = 0;
                foreach (var price in resourcePrices)
                {
                    if (price.TryPurchaseWithResourcePrice()) pricesMet++;
                }

                if (pricesMet != resourcePrices.Length) // check if player can afford the resource prices
                {
                    hexPlaceFailure = true;
                    return;
                }
                
                for (int i = 0; i < pricesMet; i++) 
                {
                    resourcePrices[i].Resource.AddAmountToResource(-resourcePrices[i].Cost);
                }
                
                GameObject hexPlaced = Instantiate(hexPrefabSelected, hexPos.ToWorld(), Quaternion.identity);
                OnHexPlaced(hexPos);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                BuildMode = false;
            }
        }

        if (Input.GetKeyDown(GATE_HEX.KeyCode))
        {
            BuildMode = true;
            CreateBluePrintHex(GATE_HEX);
        }
        if (Input.GetKeyDown(POWER_HEX.KeyCode))
        {
            BuildMode = true;
            CreateBluePrintHex(POWER_HEX);
        }
        if (Input.GetKeyDown(WORKSHOP_HEX.KeyCode))
        {
            BuildMode = true;
            CreateBluePrintHex(WORKSHOP_HEX);
        }
        if (Input.GetKeyDown(GOLDMINE_HEX.KeyCode))
        {
            BuildMode = true;
            CreateBluePrintHex(GOLDMINE_HEX);
        }
        if (Input.GetKeyDown(STAR_COLLECTOR_HEX.KeyCode))
        {
            BuildMode = true;
            CreateBluePrintHex(STAR_COLLECTOR_HEX);
        }

        void CreateBluePrintHex(HexTileType hexType)
        {
            hexTypeSelected = hexType;
            hexPrefabSelected = hexTileTypePrefabs[hexType.PrefabID];
            hexColor = hexType.HexColor;
            resourcePrices = hexType.ResourcePrices;
            StartCoroutine(BluePrintHexBlink());
        }
    }

    private void OnBuildMode()
    {
        Vector3 hexPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        hexPos = new Vector3(hexPos.x, hexPos.y, 0);
        bluePrintHex = Instantiate(hexExample, hexPos, Quaternion.identity);
        bluePrintHexsr = bluePrintHex.GetComponent<SpriteRenderer>();
    }

    private void OnBuildModeOff()
    {
        Destroy(bluePrintHex);
    }

    private void OnHexPlaced(Hex hexPos)
    {
        occupiedHexes.Add(hexPos);
    }

    private bool IsHexOccupied(Hex hexPos)
    {
        foreach (Hex hex in occupiedHexes)
        {
            if (hexPos.Equals(hex))
            {
                return true;
            }
        }
        
        return false;
    }

    private bool HasNeighborHex(Hex hexPos)
    {
        IEnumerable<Hex> neighbours = hexPos.Neighbours();
        foreach (Hex hex in occupiedHexes)
        {
            foreach (Hex neighbour in neighbours)
            {
                if (hex.Equals(neighbour)) return true;
            }
        }
        
        return false;
    }

    private float speed = 6.47f;
    private float constant = -0.531f;

    private IEnumerator BluePrintHexBlink()
    {
        while (BuildMode)
        {
            float time = 0;
            float whiteTintMax = .7f;

            // float rDifference = 1 - hexColor.r;
            // float gDifference = 1 - hexColor.g;
            // float bDifference = 1 - hexColor.b;
    
            while (!resetColor && bluePrintHexsr != null) // add clamp
            {
                float rgbWhiteTint = .5f * constant * Mathf.Cos(speed * time) + whiteTintMax * -constant;

                //bluePrintHexsr.color = new Color(hexColor.r + rDifference * (1 - rgbWhiteTint), hexColor.g + gDifference * (1 - rgbWhiteTint), 
                //    hexColor.b + bDifference * (1 - rgbWhiteTint));
                
                bluePrintHexsr.color = new Color(hexColor.r + rgbWhiteTint, hexColor.g + rgbWhiteTint, 
                    hexColor.b + rgbWhiteTint);

                if (hexPlaceFailure)
                {
                    Color lastColor = bluePrintHexsr.color;
                    bluePrintHexsr.color = failedHexPlaceColor;

                    yield return redDuration;

                    bluePrintHexsr.color = lastColor;
                    hexPlaceFailure = false;
                }

                time += Time.deltaTime;
                yield return null;
            }
        
            resetColor = false;
            yield return null;
        }
    }
}

public class HexTileType
{
    public string TileName { get; private set; }
    public Color HexColor { get; private set; }
    public int PrefabID { get; private set; }

    public ResourcePrice[] ResourcePrices { get; private set; }
    public KeyCode KeyCode { get; private set; }

    //Remember prefab must be assigned at runtime
    public HexTileType(string hexTileName, Color hexColor, int hexPrefabID, ResourcePrice[] resourcePrices = null, KeyCode hexKeyCode = KeyCode.None)
    {
        TileName = hexTileName;
        HexColor = hexColor;
        PrefabID = Mathf.Clamp(hexPrefabID, 0, 99);
        ResourcePrices = resourcePrices;
        KeyCode = hexKeyCode;
    }
}
