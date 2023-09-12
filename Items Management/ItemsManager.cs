using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    
    public enum ItemType{Item,Weapon,Armor}
    public ItemType itemType;

    public string itemName,itemDescription;
    public int valueInCoins;
    public Sprite itemsImage;

    public enum AffectType{HP,Mana}
    public int amountOfAffect;
    public AffectType affectType;
    public int weaponDexterity;
    public int armorDefence;

    public bool isStackable;

    public int amount;
    public static ItemsManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseItem(int characterToUseOn)
    {
      PlayerStats selectedCharacter = GameManager.instace.GetPlayerStats()[characterToUseOn];
       if(itemType == ItemType.Item)
       { 
        if(affectType == AffectType.HP)
        {
        selectedCharacter.AddHP(amountOfAffect);
        }else if(affectType == AffectType.Mana)
       {
       selectedCharacter.AddMana(amountOfAffect);
           
       }
       }else if(itemType == ItemType.Weapon)
       {
         if(selectedCharacter.equippedWeaponName != "")
         {
          Inventory.instance.AddItems(selectedCharacter.equippedWeapon);
         }
         selectedCharacter.EquippedWeapon(this);
       }else if(itemType == ItemType.Armor)
       {
         if(selectedCharacter.equippedArmorName != "")
         {
          Inventory.instance.AddItems(selectedCharacter.equippedArmor);
         }
         selectedCharacter.EquippedArmor(this);
       }
       
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
      if(collision.CompareTag("Player"))
      {
        //print("This item is: " + itemName);
        AudioManager.instance.PlaySFX(5);
        Inventory.instance.AddItems(this);
        SelfDestroy();
      }  
    }
    public void SelfDestroy()
    {
       gameObject.SetActive(false);
    }
}
