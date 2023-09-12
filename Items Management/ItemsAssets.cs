using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsAssets : MonoBehaviour
{

    public static ItemsAssets instance;
    [SerializeField] ItemsManager[] itemsAvailable;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public ItemsManager GetItemAsset(string itemGetName)
    {
        foreach(ItemsManager item in itemsAvailable)
        {
            if(item.itemName == itemGetName)
            {
                return item;
            }

        }
        return null;
    }
}
