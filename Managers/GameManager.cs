using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instace;

    public bool gameMenuOpened, dialogBoxOpened, shopOpened, battleiSActive;
    public int currentGold;

    [SerializeField] PlayerStats[] playerStats;
    // Start is called before the first frame update
    void Start()
    {
        if(instace != null && instace != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instace = this;
        }
        DontDestroyOnLoad(gameObject);

        playerStats = FindObjectsOfType<PlayerStats>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Data has been saved");
            SaveData();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Data has been Loaded");
            LoadData();
        }
        if(gameMenuOpened || dialogBoxOpened || shopOpened || battleiSActive)
        {
            Player.instace.deactivateMovement = true;
        }else
        {
            Player.instace.deactivateMovement = false;

        }
    }
    public PlayerStats[] GetPlayerStats()
    {
        return playerStats;
    }
    public void SaveData()
    {
        
       SavingPlayerStats();
       SavingPlayerPosition();
       PlayerPrefs.SetString("Current_Scene",SceneManager.GetActiveScene().name);

       PlayerPrefs.SetInt("Number_Of_Items",Inventory.instance.GetItemsList().Count);
       for(int i = 0; i < Inventory.instance.GetItemsList().Count; i++)
       {
        ItemsManager itemInInventory = Inventory.instance.GetItemsList()[i];
        PlayerPrefs.SetString("Item_"+i+"_Name", itemInInventory.itemName);

        if(itemInInventory.isStackable)
        {
            PlayerPrefs.SetInt("Item_"+i+"_Amount", itemInInventory.amount);
        }
       } 

            
    }
    private static void SavingPlayerPosition()
    {
        PlayerPrefs.SetFloat("Player_Pos_X", Player.instace.transform.position.x);
        PlayerPrefs.SetFloat("Player_Pos_Y", Player.instace.transform.position.y);
        PlayerPrefs.SetFloat("Player_Pos_Z", Player.instace.transform.position.z);
    }
    private void SavingPlayerStats()
    {
         for(int i=0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_"+playerStats[i].playerName+"_active",1);

            }
            else
            {
                PlayerPrefs.SetInt("Player_"+playerStats[i].playerName+"_active",0);

            }
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_Level",playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_CurrentXP",playerStats[i].currentXP);

            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_MaxHP",playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_CurrentHP",playerStats[i].currentHP);

            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_MaxMana",playerStats[i].maxMana);
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_CurrentMana",playerStats[i].currentMana);

            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_Dexterity",playerStats[i].dexterity);
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_Defence",playerStats[i].defence);

            PlayerPrefs.SetString("Player_"+playerStats[i].playerName + "_EquipedWeapon",playerStats[i].equippedWeaponName);
            PlayerPrefs.SetString("Player_"+playerStats[i].playerName + "_EquippedArmor",playerStats[i].equippedArmorName);

            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_WeaponPower",playerStats[i].weaponPower);
            PlayerPrefs.SetInt("Player_"+playerStats[i].playerName + "_ArmorDefence",playerStats[i].armorDefence);
        }
    }
   public void LoadData()
{
    LoadingPlayerPosition();
    LoadingPlayerStats();

    for(int i = 0; i < PlayerPrefs.GetInt("Number_Of_Items");i++)
    {
        string itemName = PlayerPrefs.GetString("Item_"+i+"_Name");
        ItemsManager itemToAdd = ItemsAssets.instance.GetItemAsset(itemName);
        int itemAmount = 0;

        if(PlayerPrefs.HasKey("Item_"+i+"_Amount"))
        {
            itemAmount = PlayerPrefs.GetInt("Item_"+i+"_Amount");
        }

        // Check if inventory is null before calling its methods
        if (Inventory.instance != null)
        {
            Inventory.instance.AddItems(itemToAdd);
            if(itemToAdd.isStackable && itemAmount > 1)
            {
                itemToAdd.amount = itemAmount;
            }
        }
        else
        {
            Debug.LogWarning("Inventory is not initialized.");
        }
    }
}

    private void LoadingPlayerStats()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }
            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Level", playerStats[i].playerLevel);
            playerStats[i].currentXP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentXP");

            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxHP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentHP");

            playerStats[i].maxMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxMana");
            playerStats[i].currentMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentMana");

            playerStats[i].dexterity = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Dexterity");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Defence");

            playerStats[i].equippedWeaponName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_EquipedWeapon");
            playerStats[i].equippedArmorName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_EquippedArmor");

            playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_WeaponPower");
            playerStats[i].armorDefence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_ArmorDefence");
        }
    }

    private static void LoadingPlayerPosition()
    {
        Player.instace.transform.position = new Vector3(
            PlayerPrefs.GetFloat("Player_Pos_X", Player.instace.transform.position.x),
            PlayerPrefs.GetFloat("Player_Pos_Y", Player.instace.transform.position.y),
            PlayerPrefs.GetFloat("Player_Pos_Z", Player.instace.transform.position.z)
        );
    }
}
