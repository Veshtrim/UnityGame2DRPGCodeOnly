using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] string transitionName;
    [SerializeField] AreaEnter theAreaEnter;

    // Start is called before the first frame update
    void Start()
    {
        theAreaEnter.transitionName = transitionName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
         if(collision.CompareTag("Player"))
         {
            Player.instace.transitionName = transitionName;
            MenuManager.instance.FadeImage();
            StartCoroutine(LoadSceneCoroutine());
            AudioManager.instance.PlaySFX(0);
         }
    }
    IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);

    }
}
