using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMagicButtons : MonoBehaviour
{
    public string spellName;
    public int spellCost;


    public TextMeshProUGUI spellNameText , spellCostText;
    // Start is called before the first frame update
    public void Press()
    {
        if(BattleManager.instance.GetCurrentActiveCharacter().currentMana >= spellCost)
        {
        BattleManager.instance.magicChoicePanel.SetActive(false);
        BattleManager.instance.OpenTargetMenu(spellName);
        BattleManager.instance.GetCurrentActiveCharacter().currentMana -= spellCost;

        }
        else
        {
            BattleManager.instance.battleNotice.SetText("We Dont have Mana");
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicChoicePanel.SetActive(false);
        }
    }
}
