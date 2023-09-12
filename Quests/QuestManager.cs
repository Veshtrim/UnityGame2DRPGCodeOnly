using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] string[] questNames;
    [SerializeField] bool[] questMarkersCompleted;
    
    public static QuestManager instance;
    // Start is called before the first frame update
    void Start()
    {
        questMarkersCompleted = new bool[questNames.Length];
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Data has been saved");
            SaveQuestData();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Data has been Loaded");
            LoadQuestData();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            print(CheckIfComplete("Defeat Dragon"));
            MakrQuestComplete("Steal The Gem");
            MakrQuestInComplete("Take Monster Soul");
        }
    }
    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            if(questNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogWarning("Quest: " + questToFind + "does not exist");
        return 0;
    }

    public bool CheckIfComplete(string questToCheck)
    {
       int questNumberToCheck = GetQuestNumber(questToCheck);

       if(questNumberToCheck != 0)
       {
        return questMarkersCompleted[questNumberToCheck];
       }
       return false;
    }
    public void UpdateQuestObjects()
    {
        QuestObject[] questObjects = FindObjectsOfType<QuestObject>();

        if(questObjects.Length > 0)
        {
            foreach(QuestObject questObject in questObjects)
            {
                questObject.CheckForCompletion();
            }
        }
    }
    public void MakrQuestComplete(string questToMark)
    {
      int questNumberToCheck = GetQuestNumber(questToMark);
      questMarkersCompleted[questNumberToCheck] = true;

      UpdateQuestObjects();
    }
    public void MakrQuestInComplete(string questToMark)
    {
      int questNumberToCheck = GetQuestNumber(questToMark);
      questMarkersCompleted[questNumberToCheck] = false;

      UpdateQuestObjects();
    }

    public void SaveQuestData()
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            if(questMarkersCompleted[i])
            {
                PlayerPrefs.SetInt("QuestMarker_"+questNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_"+questNames[i], 0);

            }
        }
    }
    public void LoadQuestData()
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            int valueToSet = 0;
            string keyToUse = "QuestMarker_"+questNames[i];

            if(PlayerPrefs.HasKey(keyToUse))
            {
                valueToSet = PlayerPrefs.GetInt(keyToUse);
            }

            if(valueToSet == 0)
            {
                questMarkersCompleted[i] = false;
            }
            else
            {
                questMarkersCompleted[i] = true;
            }
        }
    }
}
