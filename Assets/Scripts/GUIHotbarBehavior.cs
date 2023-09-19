using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIHotbarBehavior : MonoBehaviour
{
    private readonly Color[] iconColors =
    {
        HexBuilder.GATE_HEX.HexColor,
        HexBuilder.POWER_HEX.HexColor,
        HexBuilder.WORKSHOP_HEX.HexColor,
        HexBuilder.GOLDMINE_HEX.HexColor,
        HexBuilder.STAR_COLLECTOR_HEX.HexColor,
        HexBuilder.TURRET_HEX.HexColor
    };
    
    [SerializeField] private Image[] hotbarHexIcons = new Image[6];
    [SerializeField] private RectTransform hexListButtonManager = null;
    [SerializeField] private RectTransform shopButtonManager = null;
    
    private GUIHexListButtonBehavior hexListButtonBehaviorcs;
    private GUIShopButtonBehavior shopButtonBehaviorcs;
    
    public event Action<int> OnHotbarSlotClicked;
    public event Action<bool> OnHoveredOverHotbar;

    private bool slotsReplaceable = false;

    private void Start()
    {
        for (int i = 0; i < hotbarHexIcons.Length; i++)
        {
            SetHotbarSlotColor(i, iconColors[i]);
        }
        
        hexListButtonBehaviorcs = hexListButtonManager.GetComponent<GUIHexListButtonBehavior>();
        hexListButtonBehaviorcs.OnHexListButtonClicked += HexListButtonClicked;

        shopButtonBehaviorcs = shopButtonManager.GetComponent<GUIShopButtonBehavior>();
        shopButtonBehaviorcs.OnShopGUIOpened += ShopGUIOpened;
    }


    public void SetHotbarSlotColor(int slotNum)
    {
        slotsReplaceable = false;
        hotbarHexIcons[slotNum].color = iconColors[slotNum];
    }
        
    public void SetHotbarSlotColor(int slotNum, Color col)
    {
        slotsReplaceable = false;
        iconColors[slotNum] = col;
        hotbarHexIcons[slotNum].color = col;
    }

    // called through 6 buttons on the GUI
    public void HotbarSlotClicked(int slotNum)
    {
        slotsReplaceable = false;
        OnHotbarSlotClicked?.Invoke(slotNum);
    }
    
    private void OnMouseEnter()
    {
        OnHoveredOverHotbar?.Invoke(true);
    }

    private void OnMouseExit()
    {
        OnHoveredOverHotbar?.Invoke(false);
    }

    private void HexListButtonClicked(int hexIDSelected)
    {
        slotsReplaceable = true;
        StartCoroutine(Blink());
    }

    private void ShopGUIOpened(bool shopOpened) => slotsReplaceable = shopOpened;

    private IEnumerator Blink()
    {
        float t = 0;
        float blinkSpeed = 3f;

        while (slotsReplaceable)
        {
            float y = -0.5f * Mathf.Cos(t * blinkSpeed) + .5f;

            for (int i = 0; i < 3; i++)
            {
                hotbarHexIcons[i].color = Color.Lerp(iconColors[i], Color.white, y);
            }
            
            t += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < 3; i++)
        {
            SetHotbarSlotColor(i);
        }
    }

    private void OnDisable()
    {
        hexListButtonBehaviorcs.OnHexListButtonClicked -= HexListButtonClicked;
        shopButtonBehaviorcs.OnShopGUIOpened -= ShopGUIOpened;
    }
}
