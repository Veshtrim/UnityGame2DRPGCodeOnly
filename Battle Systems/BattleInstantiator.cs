using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInstantiator : MonoBehaviour
{
    [SerializeField] BattleTypeManager[] avaliableBattles;

    [SerializeField] bool activeOnEnter;
    private bool inArea;
    [SerializeField] float timeBetweenBattles;
    private float battleCounter;
    [SerializeField] bool deactivateAfterStarting;
    [SerializeField] bool canRunAway;

    [SerializeField] bool shouldCompleteQuest;
    public string questToComplete;
    private void Start() {
        battleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }
    private void Update() {
        if(inArea && !Player.instace.deactivateMovement)
        {
            if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0)
            {
                battleCounter -= Time.deltaTime;
            }
        }
        if(battleCounter <= 0)
        {
            battleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
            StartCoroutine(StartBattleCoroutine());
        }
    }

    private IEnumerator StartBattleCoroutine()
    {
        MenuManager.instance.FadeImage();
        GameManager.instace.battleiSActive = true;

        int selectBattle = Random.Range(0,avaliableBattles.Length);

        BattleManager.instance.itemsReward = avaliableBattles[selectBattle].rewardItems;
        BattleManager.instance.XPRewardAmount = avaliableBattles[selectBattle].rewardXP;

        BattleRewardsHanlder.instance.markQuestComplete = shouldCompleteQuest;
        BattleRewardsHanlder.instance.questToComplete = questToComplete;

        yield return new WaitForSeconds(1f);

        MenuManager.instance.FadeOut();
        BattleManager.instance.StartBattle(avaliableBattles[selectBattle].enemies,canRunAway);

        if(deactivateAfterStarting)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            if(activeOnEnter)
            {
                StartCoroutine(StartBattleCoroutine());
            }
            else
            {
                inArea = true;
            }
        }    
    }
}
