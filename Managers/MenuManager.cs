using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    public GameObject menu;
    [SerializeField] GameObject[] statsButtons;
    [SerializeField] TextMeshProUGUI statName, statHP, statMana, statDex, statDef, statEquippedWeapon, statEquippedArmor;
    [SerializeField] TextMeshProUGUI statEquippedWeaponPower, statEquippedArmorDefence;
    [SerializeField] Image characterStatImage;
    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;
    
    

    public static MenuManager instance;
    private PlayerStats[] playerStats;
    [SerializeField] TextMeshProUGUI[] nameText, hpText, manaText, xpText, currentXPText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] Image[] characterImage;
    [SerializeField] GameObject[] characterPanel;

    public TextMeshProUGUI itemName, itemDescription;

    public ItemsManager activeItem;
    [SerializeField] GameObject characterChoicePanel;
    [SerializeField] TextMeshProUGUI[] itemsCharacterChoiceNames;

    // Start is called before the first frame update

    private void Start() {
        instance = this;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.M))
        {
            UpdateItemsInvenotry();
        if(menu.activeInHierarchy)
        {
            
            
            menu.SetActive(false);
            GameManager.instace.gameMenuOpened = false;
        }else
        { 
            UpdateStats();
            menu.SetActive(true);
            GameManager.instace.gameMenuOpened = true;

        }
        }
    }
    public void UpdateStats()
    {
      playerStats = GameManager.instace.GetPlayerStats();
      for(int i  = 0;i < playerStats.Length; i++)
      {
        characterPanel[i].SetActive(true);

        nameText[i].text = playerStats[i].playerName;
        hpText[i].text = "HP: "+playerStats[i].currentHP + "/" + playerStats[i].maxHP;
        manaText[i].text = "Mana: "+playerStats[i].currentMana + "/" + playerStats[i].maxMana;
        currentXPText[i].text = "Current XP: "+playerStats[i].currentXP;
        
        characterImage[i].sprite = playerStats[i].characterImage;
          
        xpText[i].text = playerStats[i].currentXP.ToString() + "/" + playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
        xpSlider[i].maxValue = playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
        xpSlider[i].value = playerStats[i].currentXP;
      }
    }

    public void StatsMenu(){
        for(int i = 0; i<playerStats.Length; i++)
        {
         statsButtons[i].SetActive(true);

         statsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerStats[i].playerName;
        }
        StatsMenuUpdate(0);
    }
    public void StatsMenuUpdate(int playerSelectedNumber)
    {
       //PlayerStats playerSelected = playerStats[playerSelectedNumber];
       statName.text = playerStats[playerSelectedNumber].playerName;
       
       statHP.text = playerStats[playerSelectedNumber].currentHP.ToString() + "/" + playerStats[playerSelectedNumber].maxHP;
       statMana.text = playerStats[playerSelectedNumber].currentMana.ToString() + "/" + playerStats[playerSelectedNumber].maxMana;
       
       statDex.text = playerStats[playerSelectedNumber].dexterity.ToString();
       statDef.text = playerStats[playerSelectedNumber].defence.ToString();

       characterStatImage.sprite = playerStats[playerSelectedNumber].characterImage;

       statEquippedWeapon.text = playerStats[playerSelectedNumber].equippedWeaponName;
       statEquippedArmor.text = playerStats[playerSelectedNumber].equippedArmorName;

       statEquippedWeaponPower.text = playerStats[playerSelectedNumber].weaponPower.ToString();
       statEquippedArmorDefence.text = playerStats[playerSelectedNumber].armorDefence.ToString();

    }
    
    public void UpdateItemsInvenotry()
    {
        foreach(Transform itemSlot in itemSlotContainerParent)
        {
           
        Destroy(itemSlot.gameObject);
   
        }
        foreach(ItemsManager item in Inventory.instance.GetItemsList())
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();
           
            Image itemImage = itemSlot.Find("Image").GetComponent<Image>();
            itemImage.sprite = item.itemsImage;
           
           TextMeshProUGUI itemsAmountText = itemSlot.Find("Amount Text").GetComponent<TextMeshProUGUI>();
           if(item.amount >1)
              itemsAmountText.text = item.amount.ToString();
           else
              itemsAmountText.text = "";
            
           itemSlot.GetComponent<ItemButton>().itemOnButton = item;
        }
        
    }
    public void DiscardItem()
    {
        Inventory.instance.RemoveItem(activeItem);
        UpdateItemsInvenotry();
        AudioManager.instance.PlaySFX(3);
    }
    public void UseItem(int selectedCharacter)
    {
        activeItem.UseItem(selectedCharacter);
        OpenCharacterChoicePanel();
        Inventory.instance.RemoveItem(activeItem);
        UpdateItemsInvenotry();
        AudioManager.instance.PlaySFX(8);
    }
    public void OpenCharacterChoicePanel()
    {
       characterChoicePanel.SetActive(true);
       if(activeItem){ 
       for(int i = 0; i < playerStats.Length; i++)
       {
        PlayerStats activePlayer = GameManager.instace.GetPlayerStats()[i];
        itemsCharacterChoiceNames[i].text = activePlayer.playerName;

        bool activePlayerAvaliable = activePlayer.gameObject.activeInHierarchy;
        itemsCharacterChoiceNames[i].transform.parent.gameObject.SetActive(activePlayerAvaliable);
       }
       }
    }
    public void CloseCharacterChoicePanel()
    {
       characterChoicePanel.SetActive(false);

    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("We have quit the game");
    }
    public void FadeImage()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("Start Fading");

    }
    public void FadeOut()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("End Fading");

    }
    
}
