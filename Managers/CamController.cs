using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{

    private Player playerTarget;
    CinemachineVirtualCamera virtualCamera;
    [SerializeField] int musicToPlay;
    private bool musicAlreadyPlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerTarget = FindObjectOfType<Player>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = playerTarget.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(!musicAlreadyPlayer)
        {
            musicAlreadyPlayer = true;
            AudioManager.instance.PlayBackgroundMusic(musicToPlay);
        }

        while(playerTarget == null)
        {
            playerTarget = FindObjectOfType<Player>();
            if(virtualCamera)
            {
                virtualCamera.Follow = playerTarget.transform;
            }
        }
        
    }
}
