using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _LevelText;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.HasKey("Current level"))
        {
            _LevelText.text = "Current Level: " + PlayerPrefs.GetInt("Current level");
        }
        else
        { 
            PlayerPrefs.SetInt("Current level", 1);
            _LevelText.text = "Current Level: " + 1;
        }
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }
    public void TutorialPressed()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
