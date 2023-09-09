using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIShopButtonBehavior : MonoBehaviour
{
    private bool shopActive = false;
    [SerializeField] private GameObject shopGUI = null;

    private bool doubleClickTimeActive = false;
    private WaitForSeconds doubleClickDelay = new (.5f);
    
    public void ShopButtonClicked()
    {
        shopGUI.SetActive(!shopActive);
        shopActive = !shopActive;
    }

    public void HotbarShopButtonClicked()
    {
        if (!doubleClickTimeActive) StartCoroutine(ClickDelay());
        else ShopButtonClicked();
    }

    private IEnumerator ClickDelay()
    {
        doubleClickTimeActive = true;
        
        yield return doubleClickDelay;

        doubleClickTimeActive = false;
    }
}
