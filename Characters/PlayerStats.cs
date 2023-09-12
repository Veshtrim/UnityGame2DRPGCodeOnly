using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public string playerName;
    public Sprite characterImage;

    [SerializeField] int maxLevel = 50;
    public int playerLevel = 1;
    public int currentXP;
    public int[] xpForNextLevel;
    [SerializeField] int baseLevelXp = 100;
    public int maxHP = 100;
    public int currentHP;
    public int maxMana = 30;
    public int currentMana;
    public int dexterity;
    public int defence;

    public string equippedWeaponName;
    public string equippedArmorName;
    public int weaponPower;
    public int armorDefence;
 
    public ItemsManager equippedWeapon, equippedArmor;



    // Start is called before the first frame update
    void Start()
    { 
        instance = this;
        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXp;

        for(int i = 2; i < xpForNextLevel.Length; i++)
        {
            //print("We are at: "+ i);
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i + 105.6f * i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
           AddXp(100);
        }
    }

    public void AddXp(int amountOfXp)
    {
        currentXP += amountOfXp;
        if(currentXP > xpForNextLevel[playerLevel])
        {
          currentXP -= xpForNextLevel[playerLevel];
          playerLevel++;
          if(playerLevel % 2 == 0)
          {
             dexterity++;
          }else
          {
            defence++;
          }
          
          maxHP = Mathf.FloorToInt(maxHP * 1.18f);
          currentHP = maxHP;

          maxMana = Mathf.FloorToInt(maxMana * 1.06f);
          currentMana = maxMana;
          
        }
    }
    public void AddHP(int amountHPToAdd)
    {
      currentHP += amountHPToAdd;
      if(currentHP > maxHP)
      {
         currentHP = maxHP;
      }
    }
     public void AddMana(int amountManaToAdd)
    {
      currentMana += amountManaToAdd;
      if(currentMana > maxMana)
      {
         currentMana = maxMana;
      }
    }
    public void EquippedWeapon(ItemsManager weaponToEquip)
    {
        equippedWeapon = weaponToEquip;
        equippedWeaponName = equippedWeapon.itemName;
        weaponPower = equippedWeapon.weaponDexterity;
    }
    public void EquippedArmor(ItemsManager armorToEquip)
    {
        equippedArmor = armorToEquip;
        equippedArmorName = equippedArmor.itemName;
        armorDefence = equippedArmor.armorDefence;
    }
}
