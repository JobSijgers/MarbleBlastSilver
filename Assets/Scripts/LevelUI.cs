using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _LevelText;
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.LevelCompleteEvent += OnLevelComplete;
    }
    private void OnDestroy()
    {
        _gameManager.LevelCompleteEvent -= OnLevelComplete;
    }
    private void OnLevelComplete(int levelID)
    {
        _LevelText.text = "Level: " + levelID;
    }
}
