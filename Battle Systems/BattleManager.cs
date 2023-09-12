//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private bool isBattleActive;
    [SerializeField] GameObject battleScene;
    [SerializeField] List<BattleCharacters> activeCharacters = new List<BattleCharacters>();
    [SerializeField] Transform[] playerPosition, enemiesPosition;
    
    [SerializeField] BattleCharacters[] playerPrefabs, enemiesPrefabs;
    [SerializeField] int currentTurn;
    [SerializeField] bool waitingForTurn;
    [SerializeField] GameObject UIButtonHolder;
    [SerializeField] BattleMoves[] battleMovesList;
    [SerializeField] ParticleSystem characterAttackEffect;
    [SerializeField] CharacterDamageGUI damageText;
    [SerializeField] GameObject[] playersBattleStats;
    [SerializeField] TextMeshProUGUI[] playerNameText;
    [SerializeField] Slider[] playerHealthSlider, playerManaSlider;
    [SerializeField] GameObject enemyTargetPanel;
    [SerializeField] BattleTargetButtons[] targetButtons;
    public GameObject magicChoicePanel;
    [SerializeField] BattleMagicButtons[]  magicButton;
    public BattleNotification battleNotice;

    [SerializeField] float chanceToRunAway = 0.5f;
    [SerializeField] GameObject itemsToUseMenu;
    [SerializeField] ItemsManager selectedItem;
    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainersParent;
    [SerializeField] TextMeshProUGUI itemName, itemsDescription;
    [SerializeField] string gameOverScene;

    private bool runningAway;
    public int XPRewardAmount;
    public ItemsManager[] itemsReward;

    private bool canRun;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartBattle(new string[] { "Mage Master", "Warlock" }, true);
            
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }
        CheckPlayerButtonHolders();
    }

    private void CheckPlayerButtonHolders()
    {
        if (isBattleActive)
        {
            if (waitingForTurn)
            {
                if (activeCharacters[currentTurn].IsPlayer())
                    UIButtonHolder.SetActive(true);
                else
                {
                    UIButtonHolder.SetActive(false);
                    StartCoroutine(EnemyMoveCoroutine());
                }


            }
        }
    }

    public void StartBattle(string[] enemiesToSpawn,bool canRunAway)
    {
        if (!isBattleActive)
        {
            canRun = canRunAway;
            SettingUpBattle();
            AddingPlayers();
            AddingEnemies(enemiesToSpawn);
            UpdatePlayerStats();

            waitingForTurn = true;
            currentTurn = 0;//Random.Range(0,activeCharacters.Count);

        }
    }

    private void AddingEnemies(string[] enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (enemiesToSpawn[i] != "")
            {
                for (int j = 0; j < enemiesPrefabs.Length; j++)
                {
                    if (enemiesPrefabs[j].characterName == enemiesToSpawn[i])
                    {
                        BattleCharacters newEnemy = Instantiate(
                            enemiesPrefabs[j],
                              enemiesPosition[i].position,
                              enemiesPosition[i].rotation,
                              enemiesPosition[i]
                        );
                        activeCharacters.Add(newEnemy);
                    }
                }
            }
        }
    }

    private void AddingPlayers()
    {
        for (int i = 0; i < GameManager.instace.GetPlayerStats().Length; i++)
        {
            if (GameManager.instace.GetPlayerStats()[i].gameObject.activeInHierarchy)
            {
                for (int j = 0; j < playerPrefabs.Length; j++)
                {
                    if (playerPrefabs[j].characterName == GameManager.instace.GetPlayerStats()[i].playerName)
                    {
                        BattleCharacters newPlayer = Instantiate(
                          playerPrefabs[j],
                          playerPosition[i].position,
                          playerPosition[i].rotation,
                          playerPosition[i]
                        );
                        activeCharacters.Add(newPlayer);
                        ImportPlayerStats(i);
                    }
                }
            }
        }
    }

    private void ImportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instace.GetPlayerStats()[i];

        activeCharacters[i].currentHP = player.currentHP;
        activeCharacters[i].maxHP = player.maxHP;

        activeCharacters[i].currentMana = player.currentMana;
        activeCharacters[i].maxMana = player.maxMana;

        activeCharacters[i].dexterity = player.dexterity;
        activeCharacters[i].defence = player.defence;

        activeCharacters[i].weaponPower = player.weaponPower;
        activeCharacters[i].armorDefence = player.armorDefence;
    }

    private void SettingUpBattle()
    {
        isBattleActive = true;
        GameManager.instace.battleiSActive = true;

        transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            transform.position.z
        );
        battleScene.SetActive(true);
        
    }
    private void NextTurn()
    {
        currentTurn++;
        if(currentTurn >= activeCharacters.Count)
          currentTurn = 0;

          waitingForTurn = true;
          UpdateBattle();
          UpdatePlayerStats();
    }

    private void UpdateBattle()
    {
        bool allEnemiesAreDead = true;
        bool allPlayersAreDead = true;

        for(int i = 0;i < activeCharacters.Count; i++)
        {
            if(activeCharacters[i].currentHP < 0)
            {
                activeCharacters[i].currentHP = 0;
            }
            if(activeCharacters[i].currentHP == 0)
            {
                if(activeCharacters[i].IsPlayer() && !activeCharacters[i].isDead)
                {
                    activeCharacters[i].KillPlayer();
                }
                if(!activeCharacters[i].IsPlayer() && !activeCharacters[i].isDead)
                {
                    activeCharacters[i].KillEnemy();
                }
            }
            else
            {
                if(activeCharacters[i].IsPlayer())
                   allPlayersAreDead = false;
                else 
                   allEnemiesAreDead = false;   
            }
        }

        if(allEnemiesAreDead || allPlayersAreDead)
        {
            if(allEnemiesAreDead)
             StartCoroutine(EndBattleCoroutine());
            else if(allPlayersAreDead)
             StartCoroutine(GameOverCouroutine());

            // battleScene.SetActive(false);
            // GameManager.instace.battleiSActive = false;
            // isBattleActive = false;
        }
        else
        {
            while(activeCharacters[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeCharacters.Count)
                {
                    currentTurn = 0;
                }
            }
        }

    }
    
    public IEnumerator EnemyMoveCoroutine()
    {
        waitingForTurn = false;

        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    private void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].IsPlayer() && activeCharacters[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedPlayerToAttack = players[Random.Range(0, players.Count)];
        int movePower = 0;

        int selectedAttack = Random.Range(0, activeCharacters[currentTurn].AttackMovesAvailabe().Length);
        for (int i = 0; i < battleMovesList.Length; i++)
        {
            if (battleMovesList[i].moveName == activeCharacters[currentTurn].AttackMovesAvailabe()[selectedAttack])
            {


                movePower = GettingMovePowerAndEffectInstantiaton(selectedPlayerToAttack, i);
            }
        }
        //instantiate the effect on the attacking character
        InstantiateEffectOnAttackingCharacter();
        DealDamageToCharacters(selectedPlayerToAttack, movePower);

        UpdatePlayerStats();


    }

    private void InstantiateEffectOnAttackingCharacter()
    {
        Instantiate(
                    characterAttackEffect,
                    activeCharacters[currentTurn].transform.position,
                    activeCharacters[currentTurn].transform.rotation
                );
    }

    private void DealDamageToCharacters(int selectedCharacterToAttack, int movePower)
    {
        float attackPower = activeCharacters[currentTurn].dexterity + activeCharacters[currentTurn].weaponPower;
        float defenceAmount = activeCharacters[selectedCharacterToAttack].defence + activeCharacters[selectedCharacterToAttack].armorDefence;

        float damageAmount = (attackPower / defenceAmount) * movePower * Random.Range(0.9f,1.1f);
        int damageToGive = (int)damageAmount;

        damageToGive = CalculateCritical(damageToGive);

        Debug.Log(activeCharacters[currentTurn].characterName + "Just dealt "+ damageAmount + "("+damageToGive+") to " + activeCharacters[selectedCharacterToAttack]);

        activeCharacters[selectedCharacterToAttack].TakeHpDamage(damageToGive);

        CharacterDamageGUI characterDamageText = Instantiate(
         damageText,
         activeCharacters[selectedCharacterToAttack].transform.position,
         activeCharacters[selectedCharacterToAttack].transform.rotation
         
        );

        characterDamageText.SetDamage(damageToGive);
        
    }

    private int CalculateCritical(int damageToGive)
    {
         if(Random.value <= 0.8f) //this need to change at 0.1f
         {
            Debug.Log("CRITICAL HIT!! instead of " + damageToGive + " points "+(damageToGive * 2)+ "was dealt");
            return (damageToGive * 2);
         }
         return damageToGive;
    }

    public void UpdatePlayerStats()
    {
        for (int i = 0; i < playerNameText.Length; i++)
        {
            if(activeCharacters.Count > i)
            {
                if(activeCharacters[i].IsPlayer())
                {
                   BattleCharacters playerData = activeCharacters[i];

                   playerNameText[i].text = playerData.characterName;

                   playerHealthSlider[i].maxValue = playerData.maxHP;
                   playerHealthSlider[i].value = playerData.currentHP;

                   playerManaSlider[i].maxValue = playerData.maxMana;
                   playerManaSlider[i].value = playerData.currentMana;
                }
                else
                {
                    playersBattleStats[i].SetActive(false);
                }
            }
            else
            {
                playersBattleStats[i].SetActive(false);
            }
        }
    }
    public void PlayerAttack(string moveName , int selectedEnemyTarget) 
    {
        //int selectedEnemyTarget = 3;
         int movePower = 0;

         for(int i = 0; i < battleMovesList.Length; i++)
         {
            if(battleMovesList[i].moveName == moveName)
            {
                movePower = GettingMovePowerAndEffectInstantiaton(selectedEnemyTarget, i);
            }
        }

        InstantiateEffectOnAttackingCharacter();

        DealDamageToCharacters(selectedEnemyTarget,movePower);

        NextTurn();
        enemyTargetPanel.SetActive(false);
    }
    public void OpenTargetMenu(string moveName)
    {
          enemyTargetPanel.SetActive(true);

          List<int> Enemies = new List<int>();
          for(int i = 0; i < activeCharacters.Count; i++)
          {
            if(!activeCharacters[i].IsPlayer())
            {
                Enemies.Add(i);
            }
          }

          //Debug.Log(Enemies.Count);

          for(int i = 0; i < targetButtons.Length; i++)
          {
            if(Enemies.Count > i && activeCharacters[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattleTarget = Enemies[i];
                targetButtons[i].targetName.text = activeCharacters[Enemies[i]].characterName;
            }else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
          }
    }

    private int GettingMovePowerAndEffectInstantiaton(int selectChararcterTarget, int i)
    {
        int movePower;
        Instantiate(
                                  battleMovesList[i].theEffectToUse,
                                  activeCharacters[selectChararcterTarget].transform.position,
                                  activeCharacters[selectChararcterTarget].transform.rotation
                        );

        movePower = battleMovesList[i].movePower;
        return movePower;
    }
    
    public void OpenMagicPanel()
    {
        magicChoicePanel.SetActive(true);
        for(int i = 0; i < magicButton.Length; i++)
        {
            if(activeCharacters[currentTurn].AttackMovesAvailabe().Length > i)
            {
                magicButton[i].gameObject.SetActive(true);
                magicButton[i].spellName = GetCurrentActiveCharacter().AttackMovesAvailabe()[i];
                magicButton[i].spellNameText.text = magicButton[i].spellName;

                for(int j = 0; j < battleMovesList.Length; j++)
                {
                    if(battleMovesList[j].moveName == magicButton[i].spellName)
                    {
                        magicButton[i].spellCost = battleMovesList[j].manaCost;
                        magicButton[i].spellCostText.text = battleMovesList[j].manaCost.ToString();

                    }
                }

            }
            else
            {
                magicButton[i].gameObject.SetActive(false);
            }
        }
    }
  
     public BattleCharacters GetCurrentActiveCharacter()
     {
        return activeCharacters[currentTurn];
     }

     public void RunAway()
     {
        if(canRun)
        {
            if(Random.value > chanceToRunAway)
            {
                runningAway = true;

                StartCoroutine(EndBattleCoroutine());
            }
            else
            {
                NextTurn();
                battleNotice.SetText("There is no escape");
                battleNotice.Activate();
            }
        }
     }

     public void UpdateItemsInvenotry()
    {
        itemsToUseMenu.SetActive(true);
        foreach(Transform itemSlot in itemSlotContainersParent)
        {
           
        Destroy(itemSlot.gameObject);
   
        }
        foreach(ItemsManager item in Inventory.instance.GetItemsList())
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainersParent).GetComponent<RectTransform>();
           
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

    public void SelectedItemToUse(ItemsManager itemToUse)
    {
        selectedItem = itemToUse;
        itemName.text = itemToUse.itemName;
        itemsDescription.text = itemToUse.itemDescription;
    }

    public IEnumerator EndBattleCoroutine()
    {
        isBattleActive = false;
        UIButtonHolder.SetActive(false);
        enemyTargetPanel.SetActive(false);
        if(!runningAway)
        {
            battleNotice.SetText("We Won");
            battleNotice.Activate();
        }
        

        yield return new WaitForSeconds(3f);

        foreach(BattleCharacters playerInBattle in activeCharacters)
        {
            if(playerInBattle.IsPlayer())
            {
                foreach(PlayerStats playerWithStats in GameManager.instace.GetPlayerStats())
                {
                    if(playerInBattle.characterName == playerWithStats.playerName)
                    {
                        playerWithStats.currentHP = playerInBattle.currentHP;
                        playerWithStats.currentMana = playerInBattle.currentMana;
                    }
                }
            }
            Destroy(playerInBattle.gameObject);
        }
        battleScene.SetActive(false);
        activeCharacters.Clear();
        if(runningAway)
        {
            GameManager.instace.battleiSActive = false;
            runningAway = false;
        }
        else
        {
            BattleRewardsHanlder.instance.OpenRewardScreen(XPRewardAmount, itemsReward);
        }
        currentTurn = 0;
        GameManager.instace.battleiSActive = false;
    }

    public IEnumerator GameOverCouroutine()
    {
        battleNotice.SetText("We Lost");
        battleNotice.Activate();

        yield return new WaitForSeconds(3f);

        isBattleActive = false;
        
        SceneManager.LoadScene(gameOverScene);
    }
}
