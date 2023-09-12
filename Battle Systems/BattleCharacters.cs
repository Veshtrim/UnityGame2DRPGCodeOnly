using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacters : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] string[] attacksAvailable;

    public string characterName;
    public int currentHP, maxHP, currentMana, maxMana, dexterity, defence, weaponPower, armorDefence;
    public bool isDead;

    public Sprite deadSprite;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayer && isDead)
        {
            FadeOutEnemy();
        }
    }
    private void FadeOutEnemy()
    {
        GetComponent<SpriteRenderer>().color = new Color(
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.r, 1f, 0.3f * Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.g, 0f, 0.3f * Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.b, 0f, 0.3f * Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.a, 0f, 0.3f * Time.deltaTime)
        );

        if(GetComponent<SpriteRenderer>().color.a == 0)
        {
            gameObject.SetActive(false);
        }

    }

    public void KillEnemy()
    {
        isDead = true;
    }

    public bool IsPlayer(){
        return isPlayer;
    }
    public string[] AttackMovesAvailabe()
    {
        return attacksAvailable;
    }

    public void TakeHpDamage(int damageToRecive)
    {
        currentHP -= damageToRecive;

        if(currentHP < 0){
            currentHP = 0;
        }
    }

    public void KillPlayer()
    {
        if(deadSprite)
        {
            GetComponent<SpriteRenderer>().sprite = deadSprite;
            Instantiate(deadSprite,transform.position,transform.rotation);

            isDead = true;

        }
    }
}
