using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIHexListButtonBehavior : MonoBehaviour
{
    [SerializeField] private RectTransform hexDisplayIcon = null;
    [SerializeField] private TextMeshProUGUI hexNameText = null;
    [SerializeField] private TextMeshProUGUI hexInfoText = null;
    [SerializeField] private TextMeshProUGUI hexDescriptionText = null;

    private Image iconImage;

    private bool doubleClickTimeActive;
    private int lastHexID;

    private WaitForSeconds doubleClickDelay = new(.5f);
    
    public event Action<int> OnHexListButtonClicked;
    public event Action<int> OnHexDoubleClicked;
    
    private void Start()
    {
        iconImage = hexDisplayIcon.GetComponent<Image>();
    }

    public void HexButtonClicked(int hexTypeID)
    {
        HexTileType hexType = HexBuilder.HEX_TILE_TYPES[hexTypeID];

        if (!doubleClickTimeActive)
        {
            OnHexListButtonClicked?.Invoke(hexTypeID);

            iconImage.color = hexType.HexColor;
            hexNameText.text = hexType.TileName;
            hexInfoText.text = GetInfoTextFromResourcePrices(hexType.ResourcePrices);

            hexDescriptionText.text = $"General description of {hexType.TileName} (unfinished)";
            
            lastHexID = hexTypeID;
            StartCoroutine(ClickDelay());
        }
        else if (lastHexID == hexTypeID)
        {
            OnHexDoubleClicked?.Invoke(hexTypeID);
            doubleClickTimeActive = false;
        }
    }
    
    private IEnumerator ClickDelay()
    {
        doubleClickTimeActive = true;
        
        yield return doubleClickDelay;

        doubleClickTimeActive = false;
    }

    private string GetInfoTextFromResourcePrices(ResourcePrice[] resourcePrices)
    {
        if (resourcePrices.Length == 0) return "Free!";
        
        string infoText = "";
        foreach (var resourcePrice in resourcePrices)
        {
            infoText += $"-{resourcePrice.Cost} {resourcePrice.Resource.Name}\n";
        }
        
        // any extra info would need to be added here
        
        return infoText;
    }
}
