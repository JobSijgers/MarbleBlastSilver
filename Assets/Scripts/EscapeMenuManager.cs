using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _EscapeMenu;
    [Header("Sensitivity")]
    [SerializeField] private Slider _VerticalSenisitivitySlider;
    [SerializeField] private Slider _HorizontalSenisitivitySlider;
    [Header("Level Select")]
    [SerializeField] private TMP_InputField _LevelInputField;

    public delegate void SensitivityChanceDelegate(float horizontal, float vertical);
    public event SensitivityChanceDelegate SensitivityChanceEvent;

    private GameManager _gameManager;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("HorizontalSensitivity"))
        {
            PlayerPrefs.SetFloat("HorizontalSensitivity", _HorizontalSenisitivitySlider.maxValue);
        }

        if (!PlayerPrefs.HasKey("VerticalSensitivity"))
        {
            PlayerPrefs.SetFloat("VerticalSensitivity", _VerticalSenisitivitySlider.maxValue);
        }

        Invoke("ChangeSensitivity", 0.1f);
        LoadValues();

        _VerticalSenisitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        _HorizontalSenisitivitySlider.onValueChanged.AddListener(ChangeSensitivity);

        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_EscapeMenu.activeInHierarchy)
            {
                _EscapeMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                _EscapeMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
        }
    }
    private void LoadValues()
    {
        _VerticalSenisitivitySlider.value = PlayerPrefs.GetFloat("VerticalSensitivity");
        _HorizontalSenisitivitySlider.value = PlayerPrefs.GetFloat("HorizontalSensitivity");
    }
    private void SaveValues()
    {
        Debug.Log(_HorizontalSenisitivitySlider.value);
        PlayerPrefs.SetFloat("VerticalSensitivity", _VerticalSenisitivitySlider.value);
        PlayerPrefs.SetFloat("HorizontalSensitivity", _HorizontalSenisitivitySlider.value);
    }

    public void ChangeSensitivity(float newSensitivity)
    {
        if (SensitivityChanceEvent != null)
            SensitivityChanceEvent(_HorizontalSenisitivitySlider.value, _VerticalSenisitivitySlider.value);
        SaveValues();
    }
    private void ChangeSensitivity()
    {
        ChangeSensitivity(0f);
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
        _gameManager.SaveLevelCount();
        Time.timeScale = 1;
    }

    public void GoToLevelButtonPressed()
    {
        int parsedValue = int.Parse(_LevelInputField.text);
        if (parsedValue > 0)
        {
            Time.timeScale = 1;
            _gameManager.StartCoroutine(_gameManager.SetLevel(parsedValue));
        }
    }
}
