using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool _TestMode;

    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI _LevelCountText;
    [SerializeField] private Image _FadeImage;

    [Header("Generation")]
    [SerializeField] private GenerateLevel _GenerateLevel;
    [Space(10)]
    [SerializeField] private int _MinAmountOfRooms;
    [SerializeField] private int _MaxAmountOfRooms;

    [Header("Easy")]
    [SerializeField] private float _EasyLevel1Chance;
    [SerializeField] private float _EasyChanceChangePerLevel;
    [SerializeField] private float _LowestEasyChanceAfterPeak;

    [Header("Medium")]
    [SerializeField] private float _MediumLevel1Chance;
    [SerializeField] private float _MediumChanceChangePerlevelBeforePeak;
    [SerializeField] private float _MediumPeak;
    [SerializeField] private float _MediumChanceChangePerLevelAfterPeak;
    [SerializeField] private float _LowestMediumChanceAfterPeak;


    [Header("Hard")]
    [SerializeField] private float _HardLevel1Chance;
    [SerializeField] private float _HardChanceChangePerLevel;
    [SerializeField] private float _HardChanceChangePerLevelAfterMediumpeak;

    public delegate void LevelCompleteDelegate(int levelID);
    public event LevelCompleteDelegate LevelCompleteEvent;
    
    private PlayerMovement _playerMovement;
    private PowerUpManager _powerUpManager;

    private float _peakLevel;
    private float _respawnBelowY;
    private int _currentLevel;

    private Vector3 _PlayerSpawnpoint;

    private void Awake()
    {
        Instance = this;
        
    }
    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt("Current level");
        if (!_TestMode)
        {
            StartCoroutine(FadeIn(1));
            _powerUpManager = PowerUpManager.instance;
            _LevelCountText.text = "Level: " + PlayerPrefs.GetInt("Current level");
            Vector3 firstLevel = GetSegmentsForLevel(Random.Range(_MinAmountOfRooms, _MaxAmountOfRooms), _currentLevel);
            _respawnBelowY = _GenerateLevel.GenerateNewLevel((int)firstLevel.x, (int)firstLevel.y, (int)firstLevel.z, true) - 20;
        }

    }
    private void Update()
    {
        //player respawn
        if (!_TestMode)
        {
            if (Input.GetKeyDown(KeyCode.R) || _playerMovement.transform.position.y < _respawnBelowY)
            {
                _playerMovement.gameObject.transform.position = _PlayerSpawnpoint;
                _playerMovement.ResetVelocity();
            }
        }
        
    }
    private void OnApplicationQuit()
    {
        SaveLevelCount();
    }

    /// <summary>
    /// gives everything that needs the player movement the player movement
    /// </summary>
    /// <param name="movement"></param>
    public void ActivatePlayer(PlayerMovement movement)
    {
        _playerMovement = movement;
        _powerUpManager.SetPlayerMovement(movement);
    }
    /// <summary>
    /// sets respawn point of player
    /// </summary>
    /// <param name="playerSpawn"></param>
    public void SetPlayerSpawnpoint(Vector3 playerSpawn)
    {
        _PlayerSpawnpoint = playerSpawn;
    }

    public void LevelCompleted()
    {
        _currentLevel++;
        Vector3 roomsToSpawn = GetSegmentsForLevel(Random.Range(_MinAmountOfRooms,_MaxAmountOfRooms), _currentLevel);
        _respawnBelowY = _GenerateLevel.GenerateNewLevel((int)roomsToSpawn.x, (int)roomsToSpawn.y, (int)roomsToSpawn.z, false) - 20;
        if (LevelCompleteEvent != null)
        {
            LevelCompleteEvent(_currentLevel);
        }
    }
    /// <summary>
    /// Returns how many of eacht segment to spawn
    /// </summary>
    /// <returns>Returns a vector 3 where x = easy, y = medium and z = hard</returns>
    private Vector3 GetSegmentsForLevel(int PamountOfSegments, int currentLevel)
    {
        int easyRooms = 0;
        int mediumRooms = 0;
        int hardRooms = 0;
        for (int i = 0; i < PamountOfSegments; i++)
        {
            Vector3 currentChance = CalculateCurrentChance(currentLevel);
            float easyChance = currentChance.x;
            float mediumChance = currentChance.y;

            float randomFloat = Random.Range(0f, 1f);
            if (randomFloat <= easyChance / 100.0)
            {
                easyRooms++;
            }
            // Subtract the first percentage from 1 and compare with the second percentage
            else if (randomFloat <= (easyChance + mediumChance) / 100.0)
            {
                mediumRooms++;
            }
            // Output 3
            else
            {
                hardRooms++;
            }
        }
        return new Vector3(easyRooms, mediumRooms, hardRooms);
    }
    /// <summary>
    /// Calculates the current chance of each segment spawning
    /// </summary>
    /// <returns>Returns a Vector3 where x = easy, y = medium and z = hard</returns>
    private Vector3 CalculateCurrentChance(float PcurrentLevel)
    {
        //Calculates the easyChance
        float easyChance = _EasyLevel1Chance + PcurrentLevel * _EasyChanceChangePerLevel;
        //clamps the value so it doesnt go below the minimum amount
        easyChance = Mathf.Clamp(easyChance, _LowestEasyChanceAfterPeak, 100f);

        if (_MediumLevel1Chance + PcurrentLevel * _MediumChanceChangePerlevelBeforePeak < _MediumPeak)
        {
            //calculates the medium chance before the peak of the medium value
            float mediumChance = _MediumLevel1Chance + PcurrentLevel * _MediumChanceChangePerlevelBeforePeak;

            //calculates the hard chance before the peak of the medium value
            float hardChance = _HardLevel1Chance + PcurrentLevel * _HardChanceChangePerLevel;

            //sets a new peak level
            _peakLevel = PcurrentLevel;
            //returns the chance
            return new Vector3(easyChance, mediumChance, hardChance);
        }
        else
        {
            //calculates the medium chance after the peak
            float mediumChance = _MediumLevel1Chance + (_MediumPeak - (PcurrentLevel - _peakLevel) * _MediumChanceChangePerLevelAfterPeak);
            //clamps the value so it doesnt go lower than the midnium value assisgned
            mediumChance = Mathf.Clamp(mediumChance, _LowestMediumChanceAfterPeak, 100f);

            //calculates the hard chance after the peak of the medium value
            float hardChance = _HardLevel1Chance + _peakLevel * _HardChanceChangePerLevel + (PcurrentLevel - _peakLevel) * _HardChanceChangePerLevelAfterMediumpeak;
            //clamps the value so it doesnt get a total higher chance than 100%
            hardChance = Mathf.Clamp(hardChance, 0f, 100f - _LowestEasyChanceAfterPeak - _LowestMediumChanceAfterPeak);

            //returns the chance
            return new Vector3(easyChance, mediumChance, hardChance);
        }
    }
    public int GetCurrentLevel()
    {
        return _currentLevel;
    }
    public void SaveLevelCount()
    {
        PlayerPrefs.SetInt("Current level", _currentLevel);
        PlayerPrefs.Save();
    }
    public PlayerMovement GetPlayerMovement() 
    {
        return _playerMovement;
    }
    /// <summary>
    /// Sets the game to a new level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerator SetLevel(int level)
    {
        yield return StartCoroutine(FadeOut(1));
        _currentLevel = level;
        SaveLevelCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator FadeOut(float Pduration)
    {
        float t = 0;

        //changes the alpha of the FadeImage based on t
        while (t < 1)
        {
            t += Time.deltaTime / Pduration;    
            _FadeImage.color = new Color(_FadeImage.color.r, _FadeImage.color.g, _FadeImage.color.b, t);
            yield return null;
        }

        yield break;

    }
    private IEnumerator FadeIn(float Pduration)
    {
        float t = 1;
        //fade in
        while (t > 0)
        {
            t -= Time.deltaTime * Pduration;
            _FadeImage.color = new Color(_FadeImage.color.r, _FadeImage.color.g, _FadeImage.color.b, t);
            yield return null;
        }
    }
}

