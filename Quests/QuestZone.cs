using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestZone : MonoBehaviour
{  
    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;
    [SerializeField] bool markOnEnter;
    private bool canMark;
    public bool deactivateOnMarking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMark && Input.GetButtonDown("Fire1"))
        {
            canMark=false;
            MarkTheQuest();
        }
    }
    public void MarkTheQuest()
    {
        if(markAsComplete)
        {
            QuestManager.instance.MakrQuestComplete(questToMark);
        }
        else
        {
            QuestManager.instance.MakrQuestInComplete(questToMark);
        }
        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(markOnEnter)
        {
            MarkTheQuest();
        }
        else
        {
            canMark = true;
        }
    }
}
