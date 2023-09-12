using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private List<ItemsManager> itemsList;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        itemsList = new List<ItemsManager>();
        //Debug.Log("Hey a new inventory has been created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddItems(ItemsManager item)
    {
      
        if(item.isStackable)
        {
            bool itemAlredyInInventory = false;
         foreach(ItemsManager itemInInventory in itemsList)
         {
           if(itemInInventory.itemName == item.itemName)
           {
              itemInInventory.amount += item.amount;
              itemAlredyInInventory = true;
           }
         }
         if(!itemAlredyInInventory)
         {
           itemsList.Add(item);
         }
        }else
        {
       itemsList.Add(item);
        }
       //print(item.itemName + " has been added to inventory");
       //print(itemsList.Count);
    }
    public void RemoveItem(ItemsManager item)
{
    if (itemsList.Contains(item))
    {
        if (item.isStackable)
        {
            ItemsManager inventoryItem = null;

            foreach (ItemsManager itemInInventory in itemsList)
            {
                if (itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.amount--;
                    inventoryItem = itemInInventory;
                }
            }
            if (inventoryItem != null && inventoryItem.amount <= 0)
            {
                itemsList.Remove(inventoryItem);
            }
        }
        else
        {
            itemsList.Remove(item);
        }
    }
}


    public List<ItemsManager> GetItemsList()
    {
        return itemsList;
    }
}
