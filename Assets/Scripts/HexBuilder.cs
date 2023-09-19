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
    [SerializeField] private RectTransform hexListButtonManager = null;
    [SerializeField] private RectTransform hotbarManager = null;

    private PlayerControls playerControls;
    private InputAction mousePosition;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction hexSlot1InputAction;
    private InputAction hexSlot2InputAction;
    private InputAction hexSlot3InputAction;
    private InputAction hexSlot4InputAction;
    private InputAction hexSlot5InputAction;
    private InputAction hexSlot6InputAction;

    private static readonly Color CenterHexColor = new(.729f, .820f, 1, 1);
    private static readonly Color GateHexColor = new(.3208f, .3208f, .3208f, .6471f);
    private static readonly Color PowerHexColor = new(0, 0.3589f, 0.6226f, 1f);
    private static readonly Color WorkshopHexColor = new(.0218f, 0.3019f, 0, 1);
    private static readonly Color GoldmineHexColor = new(1, .733f, 0, 1);
    private static readonly Color StarCollectorHexColor = new(.71f, .286f, 0, 1);
    private static readonly Color TurretHexColor = new(.6f, .5f, 0, 1);

    public static readonly HexTileType CENTER_HEX = new("Center Hex", CenterHexColor, 0, null, true, true);
    
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
    
    public static readonly HexTileType TURRET_HEX = new("Turret Hex",  TurretHexColor, 6, 
        new ResourcePrice[] {new(ResourceManager.METAL, 200), new(ResourceManager.GOLD, 60)}, true, true);

    public static readonly HexTileType[] HEX_TILE_TYPES = {CENTER_HEX, GATE_HEX, POWER_HEX, WORKSHOP_HEX, GOLDMINE_HEX, STAR_COLLECTOR_HEX, TURRET_HEX };
    
    [FormerlySerializedAs("hexPrefabs")] [FormerlySerializedAs("hexTypes")] [SerializeField] private GameObject[] hexTileTypePrefabs;

    // an energy hex refers to any hex that either generates, transfers, or uses energy (aka any hex related to energy)
    public event Action<GameObject, Hex> OnEnergyHexPlaced;
    public event Action<Hex, GameObject> OnDisableableHexPlaced;
    public event Action OnHexBluePrintCreated;

    private Color failedHexPlaceColor = new(1, 0, 0, .5f);

    public Dictionary<Hex, GameObject> HexPosDict = new ();
    public Dictionary<Hex, GameObject> EnergyHexPosDict = new ();
    public Dictionary<Hex, GameObject> DisableableHexPosDict = new ();
    
    [HideInInspector] public GameObject CenterHex;
    private GUIHexListButtonBehavior hexListBehaviorcs;

    private GUIHotbarBehavior hotbarBehaviorcs;
    // this list can only have 6 entries, the first 3 are the default hotbar selections and the last 3 are the default recent hexes selections
    private HexTileType[] hotbarHexTypes = { GATE_HEX, POWER_HEX, WORKSHOP_HEX, GOLDMINE_HEX, STAR_COLLECTOR_HEX, TURRET_HEX };
    private GameObject hexPrefabSelected;
    private int hexTypeIDSelected;
    private bool isEnergyHex;
    private bool isDisableable;
    private bool slotReplaced;
    private bool hovering;
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
        hotbarBehaviorcs = hotbarManager.GetComponent<GUIHotbarBehavior>();
        hexListBehaviorcs = hexListButtonManager.GetComponent<GUIHexListButtonBehavior>();
        hotbarBehaviorcs.OnHotbarSlotClicked += SlotClicked;
        hotbarBehaviorcs.OnHoveredOverHotbar += HoveredOverHotbar;
        hexListBehaviorcs.OnHexListButtonClicked += HexListButtonClicked;
        hexListBehaviorcs.OnHexDoubleClicked += HexListButtonDoubleClicked;
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
        if (ManagerReferences.Instance.UIManager.ShopGUIOpen)
        {
            BuildMode = false;
            
            hexSlot1InputAction.performed -= Slot1Pressed;
            hexSlot2InputAction.performed -= Slot2Pressed;
            hexSlot3InputAction.performed -= Slot3Pressed;
            hexSlot4InputAction.performed -= Slot4Pressed;
            hexSlot5InputAction.performed -= Slot5Pressed;
            hexSlot6InputAction.performed -= Slot6Pressed;
            
            return;
        }
        
        if (!slotReplaced)
        {
            hexSlot1InputAction.performed -= ReplaceSlot1;
            hexSlot2InputAction.performed -= ReplaceSlot2;
            hexSlot3InputAction.performed -= ReplaceSlot3;

            slotReplaced = true;
        }
        
        if (BuildMode)
        {
            hexPos = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>()).SnapWorld();
            hexPos = new Vector3(hexPos.x, hexPos.y, -2);
            bluePrintHex.transform.position = hexPos;

            if (hovering)
            {
                leftClick.performed -= PlaceSelectedHex;
            }
            else
            {
                leftClick.performed += PlaceSelectedHex;
            }
            
            rightClick.performed += context => BuildMode = false;
        }
        
        hexSlot1InputAction.performed -= ReplaceSlot1;
        hexSlot2InputAction.performed -= ReplaceSlot2;
        hexSlot3InputAction.performed -= ReplaceSlot3;

        hexSlot1InputAction.performed += Slot1Pressed;
        hexSlot2InputAction.performed += Slot2Pressed;
        hexSlot3InputAction.performed += Slot3Pressed;
        hexSlot4InputAction.performed += Slot4Pressed;
        hexSlot5InputAction.performed += Slot5Pressed;
        hexSlot6InputAction.performed += Slot6Pressed;
    }

    private void HexListButtonClicked(int hexTypeID)
    {
        hexTypeIDSelected = hexTypeID;
        hexSlot1InputAction.performed += ReplaceSlot1;
        hexSlot2InputAction.performed += ReplaceSlot2;
        hexSlot3InputAction.performed += ReplaceSlot3;
    }

    private void HexListButtonDoubleClicked(int hexTypeID)
    {
        OnHexBluePrintCreated?.Invoke();
        CreateBluePrintHex(HEX_TILE_TYPES[hexTypeID]);
    }

    private void Slot1Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[0]);
    private void Slot2Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[1]);
    private void Slot3Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[2]);
    private void Slot4Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[3]);
    private void Slot5Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[4]);
    private void Slot6Pressed(InputAction.CallbackContext context) => CreateBluePrintHex(hotbarHexTypes[5]);
    
    private void ReplaceSlot1(InputAction.CallbackContext context) => ReplaceQuickSlot(0);
    private void ReplaceSlot2(InputAction.CallbackContext context) => ReplaceQuickSlot(1);
    private void ReplaceSlot3(InputAction.CallbackContext context) => ReplaceQuickSlot(2);

    private void SlotClicked(int slotNum) => CreateBluePrintHex(hotbarHexTypes[slotNum]);

    private void ReplaceQuickSlot(int hexSlotNum)
    {
        slotReplaced = false;

        // cannot be zero because that is the center hex, which should not selectable (or placeable)
        if (hexTypeIDSelected > 0 && hexTypeIDSelected < HEX_TILE_TYPES.Length)
        {
            // if another hotbar slot (0, 1, 2) has the same hex already, swap the hexes instead. if not, just set the slot to the hex selected
            for (int i = 1; i < 3; i++)
            {
                if (HEX_TILE_TYPES[hexTypeIDSelected].Equals(hotbarHexTypes[(hexSlotNum + i) % 3]))
                {
                    SwapHotbarSlots(hexSlotNum, (hexSlotNum + i) % 3);
                    return;
                }
            }

            SetHotbarSlot(hexSlotNum, HEX_TILE_TYPES[hexTypeIDSelected]);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(hexTypeIDSelected), 
                $"hexTypeID is a value out of the allowed range of HEX_TILE_TYPES: hexTypeID = {hexTypeIDSelected}");
        }
    }

    private void ReplaceRecentSlot(HexTileType hexType)
    {
        // return if hex already in a quick slot or first recent slot
        for (int i = 0; i < 6; i++) if (hexType.Equals(hotbarHexTypes[i])) return;

        SetHotbarSlot(5, hotbarHexTypes[4]);
        SetHotbarSlot(4, hotbarHexTypes[3]);
        SetHotbarSlot(3, hexType);
    }

    private void SwapHotbarSlots(int slotNum1, int slotNum2)
    {
        HexTileType temp = hotbarHexTypes[slotNum2];
        SetHotbarSlot(slotNum2, hotbarHexTypes[slotNum1]);
        SetHotbarSlot(slotNum1, temp);
    }

    private void SetHotbarSlot(int slotNum, HexTileType hexType)
    {
        hotbarHexTypes[slotNum] = hexType;
        hotbarBehaviorcs.SetHotbarSlotColor(slotNum, hexType.HexColor);
    }

    private void HoveredOverHotbar(bool hovered) => hovering = hovered;

    // Hotbar buttons should call this function with their associated hex type as the parameter
    private void CreateBluePrintHex(HexTileType hexType)
    {
        BuildMode = true;
        
        hexPrefabSelected = hexTileTypePrefabs[hexType.PrefabID];
        hexColor = hexType.HexColor;
        resourcePrices = hexType.ResourcePrices;
        isEnergyHex = hexType.IsEnergyHex;
        isDisableable = hexType.IsDisableable;

        ReplaceRecentSlot(hexType);

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
        leftClick.performed -= PlaceSelectedHex;
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

        if (isDisableable)
        {
            Debug.Log("disableable placed.");
            DisableableHexPosDict.Add(hexPos, hexObject);
            OnDisableableHexPlaced?.Invoke(hexPos, hexObject);
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
        hexSlot1InputAction = playerControls.HexBuildingActions.SelectHexSlot1;
        hexSlot2InputAction = playerControls.HexBuildingActions.SelectHexSlot2;
        hexSlot3InputAction = playerControls.HexBuildingActions.SelectHexSlot3;
        hexSlot4InputAction = playerControls.HexBuildingActions.SelectHexSlot4;
        hexSlot5InputAction = playerControls.HexBuildingActions.SelectHexSlot5;
        hexSlot6InputAction = playerControls.HexBuildingActions.SelectHexSlot6;
        playerControls.HexBuildingActions.Enable();
    }

    private void OnDisable()
    {
        hotbarBehaviorcs.OnHotbarSlotClicked -= SlotClicked;
        hotbarBehaviorcs.OnHoveredOverHotbar -= HoveredOverHotbar;
        hexListBehaviorcs.OnHexListButtonClicked -= HexListButtonClicked;
        hexListBehaviorcs.OnHexDoubleClicked -= HexListButtonDoubleClicked;
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
    public bool IsDisableable { get; private set; }
    public InputAction HexInputAction { get; set; }

    //Remember prefab must be assigned at runtime
    public HexTileType(string hexTileName, Color hexColor, int hexPrefabID, ResourcePrice[] resourcePrices = null, 
        bool isEnergyHex = false, bool isDisableable = false, InputAction hexInputAction = null)
    {
        TileName = hexTileName;
        HexColor = hexColor;
        PrefabID = Mathf.Clamp(hexPrefabID, 0, 99);
        ResourcePrices = resourcePrices;
        IsEnergyHex = isEnergyHex;
        IsDisableable = isDisableable;
        HexInputAction = hexInputAction;
    }
}
