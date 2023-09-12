using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic(5);
        Player.instace.gameObject.SetActive(false);
        MenuManager.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false);

    }
   
    // Update is called once per frame
    public void QuitToMainMenu()
    {
       DestroyGameSession();
       SceneManager.LoadScene("MainMenu");
    }
    public void LoadLastSave()
    {
       
       SceneManager.LoadScene("LoadingScene");
       DestroyGameSession();
    }
    private static void DestroyGameSession()
    {
       Destroy(GameManager.instace.gameObject);
       Destroy(Player.instace.gameObject);
       Destroy(MenuManager.instance.gameObject);
       Destroy(BattleManager.instance.gameObject);

    }
    public void QuitGame()
    {
        Debug.Log("We have quit the game");
        Application.Quit();
    }
}
