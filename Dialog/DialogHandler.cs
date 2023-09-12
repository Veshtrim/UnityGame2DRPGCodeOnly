using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    public string[] senteces;
    private bool canActivateBox;

    [SerializeField] bool shouldActivateQuest;
    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canActivateBox && Input.GetButtonDown("Fire1") && !DialogController.instace.IsDialogBoxActive())
        {
            DialogController.instace.ActivateDialog(senteces);

            if(shouldActivateQuest)
            {
                DialogController.instace.ActivateQuestAtEnd(questToMark,markAsComplete);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            canActivateBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            canActivateBox = false;
        }
    }
}
