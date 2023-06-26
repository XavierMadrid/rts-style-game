using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class HexBuilder : MonoBehaviour
{
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private GameObject hexExample = null;

    private PlayerControls playerControls = null;
    private InputAction mousePosition;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction gateHexInputAction;
    private InputAction powerHexInputAction;
    private InputAction workshopHexInputAction;
    private InputAction goldmineHexInputAction;
    private InputAction starCollectorHexInputAction;

    private static readonly Color CenterHexColor = new(.729f, .820f, 1, 1);
    private static readonly Color GateHexColor = new(.3208f, .3208f, .3208f, .6471f);
    private static readonly Color PowerHexColor = new(0, 0.3589f, 0.6226f, 1f);
    private static readonly Color WorkshopHexColor = new(.0218f, 0.3019f, 0, 1);
    private static readonly Color GoldmineHexColor = new(1, .733f, 0, 1);
    private static readonly Color StarCollectorHexColor = new(.71f, .286f, 0, 1);
    
    public static readonly HexTileType CENTER_HEX = new("Center Hex", CenterHexColor, 0, null, true);
    
    public static readonly HexTileType GATE_HEX = new("Connector Hex",  GateHexColor, 1, 
        new ResourcePrice[] {new(ResourceManager.METAL, 30)});
    
    public static readonly HexTileType POWER_HEX = new("Power Hex", PowerHexColor, 2,
        new ResourcePrice[] {new(ResourceManager.METAL, 200), new(ResourceManager.ENERGY, 120)}, true);
    
    public static readonly HexTileType WORKSHOP_HEX = new("Workshop Hex", WorkshopHexColor, 3, 
        new ResourcePrice[] {new(ResourceManager.METAL, 180), new(ResourceManager.GREENERY, 60)});
    
    public static readonly HexTileType GOLDMINE_HEX = new("Golden Hex",  GoldmineHexColor, 4, 
        new ResourcePrice[] {new(ResourceManager.METAL, 300)});
    
    public static readonly HexTileType STAR_COLLECTOR_HEX = new("Star Collector Hex",  StarCollectorHexColor, 5, 
        new ResourcePrice[] {new(ResourceManager.METAL, 200)}, true);

    [FormerlySerializedAs("hexPrefabs")] [FormerlySerializedAs("hexTypes")] [SerializeField] private GameObject[] hexTileTypePrefabs;

    // an energy hex refers to any hex that either generates, transfers, or uses energy (aka any hex related to energy)
    public event Action<GameObject, Hex> OnEnergyHexPlaced;
    
    private Color failedHexPlaceColor = new(1, 0, 0, .5f);

    public Dictionary<Hex, GameObject> HexPosDict = new ();
    public Dictionary<Hex, GameObject> EnergyHexPosDict = new ();
    [HideInInspector] public GameObject CenterHex;
    private GameObject hexPrefabSelected;
    private bool isEnergyHex;
    private Color hexColor;
    private ResourcePrice[] resourcePrices;
    private GameObject bluePrintHex;
    private SpriteRenderer bluePrintHexsr;
    private Vector3 hexPos;
    
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

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        CenterHex = Instantiate(hexTileTypePrefabs[CENTER_HEX.PrefabID], Vector3.zero, Quaternion.identity);
        HexPosDict.Add(CenterHex.transform.position.ToHex(), CenterHex);
        EnergyHexPosDict.Add(CenterHex.transform.position.ToHex(), CenterHex);
    }

    // Update is called once per frame
    void Update()
    {
        if (BuildMode)
        {
            hexPos = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>()).SnapWorld();
            hexPos = new Vector3(hexPos.x, hexPos.y, -2);
            bluePrintHex.transform.position = hexPos;
            
            leftClick.performed += PlaceSelectedHex;
            rightClick.performed += context => BuildMode = false;

            // if (Input.GetMouseButtonDown(0))
            // {
            //     Hex hexPos = mousePos.ToHex();
            //
            //     if (IsHexOccupied(hexPos) || !HasNeighborHex(hexPos)) // check hex position viability
            //     {
            //         hexPlaceFailure = true;
            //         return;
            //     }
            //
            //     int pricesMet = 0;
            //     foreach (var price in resourcePrices)
            //     {
            //         if (price.TryPurchaseWithResourcePrice()) pricesMet++;
            //     }
            //
            //     if (pricesMet != resourcePrices.Length) // check if player can afford the resource prices
            //     {
            //         hexPlaceFailure = true;
            //         return;
            //     }
            //     
            //     for (int i = 0; i < pricesMet; i++) 
            //     {
            //         resourcePrices[i].Resource.AddAmountToResource(-resourcePrices[i].Cost);
            //     }
            //     
            //     GameObject hexPlaced = Instantiate(hexPrefabSelected, hexPos.ToWorld(), Quaternion.identity);
            //     OnHexPlaced(hexPos); 
            // }
            
            // if (Input.GetMouseButtonDown(1))
            // {
            //     BuildMode = false;
            // }
        }

        gateHexInputAction.performed += context => CreateBluePrintHex(GATE_HEX);
        powerHexInputAction.performed += context => CreateBluePrintHex(POWER_HEX);
        workshopHexInputAction.performed += context => CreateBluePrintHex(WORKSHOP_HEX);
        goldmineHexInputAction.performed += context => CreateBluePrintHex(GOLDMINE_HEX);
        starCollectorHexInputAction.performed += context => CreateBluePrintHex(STAR_COLLECTOR_HEX);

        // if (Input.GetKeyDown(GATE_HEX.KeyCode))
        // {
        //     BuildMode = true;
        //     CreateBluePrintHex(GATE_HEX);
        // }
        // if (Input.GetKeyDown(POWER_HEX.KeyCode))
        // {
        //     BuildMode = true;
        //     CreateBluePrintHex(POWER_HEX);
        // }
        // if (Input.GetKeyDown(WORKSHOP_HEX.KeyCode))
        // {
        //     BuildMode = true;
        //     CreateBluePrintHex(WORKSHOP_HEX);
        // }
        // if (Input.GetKeyDown(GOLDMINE_HEX.KeyCode))
        // {
        //     BuildMode = true;
        //     CreateBluePrintHex(GOLDMINE_HEX);
        // }
        // if (Input.GetKeyDown(STAR_COLLECTOR_HEX.KeyCode))
        // {
        //     BuildMode = true;
        //     CreateBluePrintHex(STAR_COLLECTOR_HEX);
        // }
    }
    
    private void CreateBluePrintHex(HexTileType hexType)
    {
        BuildMode = true;
        
        hexPrefabSelected = hexTileTypePrefabs[hexType.PrefabID];
        hexColor = hexType.HexColor;
        resourcePrices = hexType.ResourcePrices;
        isEnergyHex = hexType.IsEnergyHex;
        
        StartCoroutine(BluePrintHexBlink());
    }

    private void PlaceSelectedHex(InputAction.CallbackContext context)
    {
        
        Hex hexPos = this.hexPos.ToHex();

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

        OnHexPlaced(hexPos, hexPlaced);
    }

    private void OnBuildMode()
    {
        Vector3 hexPos = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        hexPos = new Vector3(hexPos.x, hexPos.y, 0);
        
        bluePrintHex = Instantiate(hexExample, hexPos, Quaternion.identity);
        bluePrintHexsr = bluePrintHex.GetComponent<SpriteRenderer>();
    }

    private void OnBuildModeOff()
    {
        Destroy(bluePrintHex);
    }

    private void OnHexPlaced(Hex hexPos, GameObject hexObject)
    {
        HexPosDict.Add(hexPos, hexObject);
        
        if (isEnergyHex)
        {
            EnergyHexPosDict.Add(hexPos, hexObject);
            OnEnergyHexPlaced?.Invoke(hexObject, hexPos);
        }
    }

    private bool IsHexOccupied(Hex hexPos)
    {
        return HexPosDict.Keys.Contains(hexPos);
    }

    private bool HasNeighborHex(Hex hexPos)
    {
        IEnumerable<Hex> neighbours = hexPos.Neighbours();
        foreach (Hex hex in HexPosDict.Keys)
        {
            foreach (var neighbor in neighbours)
            {
                if (neighbor.Equals(hex))
                {
                    return true;
                }
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

            while (!resetColor && bluePrintHexsr != null) // add clamp
            {
                float rgbWhiteTint = .5f * constant * Mathf.Cos(speed * time) + whiteTintMax * -constant;

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

    private void OnEnable()
    {
        mousePosition = playerControls.HexBuildingActions.MousePosition;
        leftClick = playerControls.HexBuildingActions.PlaceSelectedHex;
        rightClick = playerControls.HexBuildingActions.ExitBuildMode;
        gateHexInputAction = playerControls.HexBuildingActions.SelectGateHex;
        powerHexInputAction = playerControls.HexBuildingActions.SelectPowerHex;
        workshopHexInputAction = playerControls.HexBuildingActions.SelectWorkshopHex;
        goldmineHexInputAction = playerControls.HexBuildingActions.SelectGoldmineHex;
        starCollectorHexInputAction = playerControls.HexBuildingActions.SelectStarCollectorHex;
        playerControls.HexBuildingActions.Enable();
    }

    private void OnDisable()
    {
        playerControls.HexBuildingActions.Disable();
    }
}

public class HexTileType
{
    public string TileName { get; private set; }
    public Color HexColor { get; private set; }
    public int PrefabID { get; private set; }
    public ResourcePrice[] ResourcePrices { get; private set; }
    public bool IsEnergyHex { get; private set; }
    public InputAction HexInputAction { get; set; }

    //Remember prefab must be assigned at runtime
    public HexTileType(string hexTileName, Color hexColor, int hexPrefabID, ResourcePrice[] resourcePrices = null, bool isEnergyHex = false, InputAction hexInputAction = null)
    {
        TileName = hexTileName;
        HexColor = hexColor;
        PrefabID = Mathf.Clamp(hexPrefabID, 0, 99);
        ResourcePrices = resourcePrices;
        IsEnergyHex = isEnergyHex;
        HexInputAction = hexInputAction;
    }
}
