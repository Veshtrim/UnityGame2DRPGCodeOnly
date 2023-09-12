using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopMenu, buyPanel, sellPanel;
    [SerializeField] TextMeshProUGUI currentGoldText;
    public List<ItemsManager> itemsForSale;
     [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotBuyContainerParent;
    [SerializeField] Transform itemSlotSellContainerParent;

    [SerializeField] ItemsManager selectedItem;
    [SerializeField] TextMeshProUGUI buyItemName, buyItemDescription, buyItemValue;
    [SerializeField] TextMeshProUGUI sellItemName, sellItemDescription, sellItemValue;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemsInShop(itemSlotBuyContainerParent, itemsForSale);
        UpdateItemsInShop(itemSlotSellContainerParent, Inventory.instance.GetItemsList());
    }

    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        GameManager.instace.shopOpened = true;

        currentGoldText.text = "Gold: " + GameManager.instace.currentGold;
        buyPanel.SetActive(true);
    }
    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        GameManager.instace.shopOpened = false;
    }

    public void OpenBuyPanel()
    {
    buyPanel.SetActive(true);
    sellPanel.SetActive(false);

    // Disable all existing item slots
    UpdateItemsInShop(itemSlotBuyContainerParent, itemsForSale);
    }
    public void OpenSellPanel()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);

        UpdateItemsInShop(itemSlotSellContainerParent, Inventory.instance.GetItemsList());
    }
  private void UpdateItemsInShop(Transform itemSlotContainerParent, List<ItemsManager> itemsToLookThrough)
{
    // Remove any null references from the itemsToLookThrough list
    itemsToLookThrough.RemoveAll(item => item == null);

    // Disable all item slots initially
    foreach (Transform itemSlot in itemSlotContainerParent)
    {
        itemSlot.gameObject.SetActive(false);
    }

    // Enable item slots for items in the itemsToLookThrough list
    for (int i = 0; i < itemsToLookThrough.Count; i++)
    {
        // If an item slot doesn't exist yet, create a new one
       if (i >= itemSlotContainerParent.childCount)
            {
                if (itemSlotContainer == null)
                {
                    Debug.LogError("Item slot container is not assigned in the Inspector.");
                    return;
                }

                RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();
                ItemButton itemButton = itemSlot.GetComponent<ItemButton>();
                itemButton.itemOnButton = itemsToLookThrough[i];
                Image originalImage = itemSlotContainer.GetComponent<Image>();
                Sprite originalSprite = originalImage.sprite;
                itemButton.GetComponent<Image>().sprite = originalSprite; // set the original sprite for the new item slot
                itemSlot.Find("Image").GetComponent<Image>().sprite = originalSprite; // set sprite for cloned item slot
            }


        else
        { 
            Transform itemSlotTransform = itemSlotContainerParent.GetChild(i);
            itemSlotTransform.gameObject.SetActive(true);
            itemSlotTransform.Find("Image").GetComponent<Image>().sprite = itemsToLookThrough[i].itemsImage;

            // Update the ItemButton component of the item slot with the correct ItemsManager instance
            itemSlotTransform.GetComponent<ItemButton>().itemOnButton = itemsToLookThrough[i];
        }

        TextMeshProUGUI itemsAmountText = itemSlotContainerParent.GetChild(i).Find("Amount Text").GetComponent<TextMeshProUGUI>();
        if (itemsToLookThrough[i].amount > 1)
        {
            itemsAmountText.text = itemsToLookThrough[i].amount.ToString();
        }
        else
        {
            itemsAmountText.text = "";
        }
    }
}

    public void SelectedBuyItem(ItemsManager itemToBuy)
    {
       selectedItem = itemToBuy;
       buyItemName.text = selectedItem.itemName;
       buyItemDescription.text = selectedItem.itemDescription;
       buyItemValue.text = "Value: " + selectedItem.valueInCoins;

    }

    public void SelectedSellItem(ItemsManager itemToSell)
    {
       selectedItem = itemToSell;
       sellItemName.text = selectedItem.itemName;
       sellItemDescription.text = selectedItem.itemDescription;
       sellItemValue.text = "Value: " + (int)(selectedItem.valueInCoins*0.75f);
    }

    public void BuyItem()
    {
        if(GameManager.instace.currentGold >= selectedItem.valueInCoins)
        {
            GameManager.instace.currentGold -= selectedItem.valueInCoins;
            Inventory.instance.AddItems(selectedItem);

            currentGoldText.text = "Gold: " + GameManager.instace.currentGold;
        }
    }

    public void SellItem()
    {
        if(selectedItem)
        {
            GameManager.instace.currentGold += (int)(selectedItem.valueInCoins*0.75f);
            Inventory.instance.RemoveItem(selectedItem);

            currentGoldText.text = "Gold: " + GameManager.instace.currentGold;
            selectedItem = null;
            OpenSellPanel();
        }
            
        
    }

}
