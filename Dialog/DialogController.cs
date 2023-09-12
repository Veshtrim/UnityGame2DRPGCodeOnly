using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class DialogController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText, nameText;
    [SerializeField] GameObject dialogBox, nameBox;
    [SerializeField] string[] dialogSentences;
    [SerializeField] int currentSentence;

    public static DialogController instace;

    private bool dialogJustStarted;

    private string questToMark;
    private bool markTheQuestComplete;
    private bool shouldMarkQuest;
    


    // Start is called before the first frame update
    void Start()
    {
        instace = this;
        dialogText.text = dialogSentences[currentSentence];
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogBox.activeInHierarchy)
        {
             if(Input.GetButtonUp("Fire1"))
             {
                if(!dialogJustStarted)
                { 
                
                currentSentence++;
                if(currentSentence >= dialogSentences.Length){
                    dialogBox.SetActive(false);
                     GameManager.instace.dialogBoxOpened = false;
                     if(shouldMarkQuest)
                     {
                        shouldMarkQuest = false;
                        if(markTheQuestComplete)
                        {
                            QuestManager.instance.MakrQuestComplete(questToMark);
                        }
                        else
                        {
                            QuestManager.instance.MakrQuestInComplete(questToMark);

                        }
                     }
                }else{
                    CheckForName();
                    dialogText.text = dialogSentences[currentSentence];

                }
                }else
                {
                    dialogJustStarted = false;
                }
                
                
             }
        }
    }
    public void ActivateQuestAtEnd(string questNames, bool markComplete)
    {
        questToMark = questNames;
        markTheQuestComplete = markComplete;
        shouldMarkQuest = true;
    }

    public void ActivateDialog(string[] newSentencesToUse)
    {
        
        dialogSentences = newSentencesToUse;
        currentSentence = 0;
        CheckForName();
        dialogText.text = dialogSentences[currentSentence];
        dialogBox.SetActive(true);

        dialogJustStarted = true;
        GameManager.instace.dialogBoxOpened = true;

    }
    void CheckForName()
    {
     if(dialogSentences[currentSentence].StartsWith("#")){
        nameText.text = dialogSentences[currentSentence].Replace("#","");
        currentSentence++;
     }
    }

    public bool IsDialogBoxActive()
    {
        return dialogBox.activeInHierarchy;
    }
}
