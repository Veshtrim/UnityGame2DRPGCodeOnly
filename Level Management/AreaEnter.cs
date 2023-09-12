using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnter : MonoBehaviour
{
    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        if(transitionName == Player.instace.transitionName)
        {
          Player.instace.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
