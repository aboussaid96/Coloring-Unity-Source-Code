using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Ui : MonoBehaviour
{
    public GameObject GamePanel;
    public GameObject LevelcompletePanel;
    public TextMeshProUGUI LevelNumber;
    public static Ui Instance;

    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }


    void Start()
    {
        GamePanel.SetActive(true);
        LevelcompletePanel.SetActive(false);
        LevelNumber.text = "Level " + PlayerPrefs.GetInt("level", 1).ToString();
        if (AdManager.instance)
        {
            AdManager.instance.loadInterstitial();
        }

    }


    void Update()
    {
        
    }
    public void LevelCompleted()
    {
        GamePanel.SetActive(false);
        LevelcompletePanel.SetActive(true);
        Controller.Instance.colorselection.SetActive(false);
        StartCoroutine(showingad());

    }
 
    IEnumerator showingad()
    {
        yield return new WaitForSeconds(1f);
        if(AdManager.instance)
        {
            AdManager.instance.showInterstitial();
        }

    }

    public void nextlevel()
    {
        if (PlayerPrefs.GetInt("level") >= (SceneManager.sceneCountInBuildSettings) - 1)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
            int i = Random.Range(1, (SceneManager.sceneCountInBuildSettings));
            PlayerPrefs.SetInt("THISLEVEL", i);
            SceneManager.LoadScene(i);
        }
        else
        {
            PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void restartlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
