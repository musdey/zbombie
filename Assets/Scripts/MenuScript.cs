using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Text highscore;

	// Use this for initialization
	void Start () {

        highscore.text = ""+PlayerPrefs.GetInt("highscore");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMenu()
    {
        SceneManager.UnloadScene("GameScene");
        SceneManager.LoadScene("MenuScene1");
        print("loadmenucalled");
    }

    public void CloseGame()
    {
        Application.Quit();
        print("close is called amk");
    }

}
