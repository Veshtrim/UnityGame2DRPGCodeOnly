using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BattleRewardsHanlder : MonoBehaviour
{
    public static BattleRewardsHanlder instance;

    [SerializeField] TextMeshProUGUI XPText, itemsText;
    [SerializeField] GameObject rewardScreen;

    [SerializeField] ItemsManager[] rewardItems;
    [SerializeField] int xpReward;

    public bool markQuestComplete;
    public string questToComplete;
    
    private void Start() 
    {
      instance = this;    
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(15000, rewardItems);
        }
    }
    public void OpenRewardScreen(int xpEarned, ItemsManager[] itemsEarned)
    {
        xpReward = xpEarned;
        rewardItems = itemsEarned;

        XPText.text = xpEarned + " XP";
        itemsText.text = "";

        foreach(ItemsManager rewardItemText in rewardItems)
        {
            itemsText.text += rewardItemText.itemName + " ";
        }

        rewardScreen.SetActive(true);
    }
   
    public void CloseRewardScreen()
    {
        foreach (PlayerStats activePlayer in GameManager.instace.GetPlayerStats())
        {
            if(activePlayer.gameObject.activeInHierarchy)
            {
                activePlayer.AddXp(xpReward);
            }
        }

        foreach (ItemsManager itemRewarded in rewardItems)
        {
            Inventory.instance.AddItems(itemRewarded);
        }

        rewardScreen.SetActive(false);
        GameManager.instace.battleiSActive = false;

        if(markQuestComplete)
        {
            QuestManager.instance.MakrQuestComplete(questToComplete);
        }
    }
}
