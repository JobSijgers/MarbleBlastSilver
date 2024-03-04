using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Material[] _LevelColors; 

    [Header("Objects")]
    [SerializeField] private GameObject _EmptyLevel;

    [Header("Folders")]
    [SerializeField] private string _SkyboxMaterialsFolder;
    [SerializeField] private string _EasySegmentsFolder;
    [SerializeField] private string _MediumSegmenetsFolder;
    [SerializeField] private string _HardSegmentsFolder;

    [Header("Player")]
    [SerializeField] private GameObject _PlayerPrefab;
    [SerializeField] private GameObject _SpawnPlatformPrefab;
    [SerializeField] private GameObject _EndPlatformPrefab;
 
    [Space(10)]
    [Header("Resources")]
    [SerializeField] private Material[] _SkyboxMaterials;
    [SerializeField] private GameObject[] _EasySegments;
    [SerializeField] private GameObject[] _MediumSegments;
    [SerializeField] private GameObject[] _HardSegments;
    
    private GameManager _gameManager;

    private Vector3 _playerSpawnPoint;
    private Vector3 _segmentSpawnpoint;

    private GameObject _activePlayer;
    private GameObject _activeSpawnPlatform;
    
    private List<GameObject> _selectedPrefabs = new List<GameObject>();

    private List<GameObject> _activeLevels = new List<GameObject>();

    private int currentColorIndex = 0;

    private float _lowestPointInLevel = 0;
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void PickRandomSkybox()
    {
        RenderSettings.skybox = _SkyboxMaterials[Random.Range(0, _SkyboxMaterials.Length)];
    }
   
    /// <summary>
    /// Spawns player
    /// </summary>
    public void SpawnPlayer()
    {
        _activePlayer = Instantiate(_PlayerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
        PlayerMovement movement = _activePlayer.GetComponentInChildren<PlayerMovement>();
        _gameManager.ActivatePlayer(movement);
    }
    /// <summary>
    /// Generates a new Level
    /// </summary>
    /// <param name="PEasyRooms">amount of easy rooms</param>
    /// <param name="PmediumRooms">amount of medium rooms</param>
    /// <param name="PhardRooms">amount of hard rooms</param>
    /// <returns> Returns the lowest y in the level</returns>
    public float GenerateNewLevel(int PEasyRooms, int PmediumRooms, int PhardRooms, bool PfirstLevel)
    {
        _lowestPointInLevel = _segmentSpawnpoint.y;
        _selectedPrefabs.Clear();
        _activeSpawnPlatform = PlaceSpawnPlatForm();
        _segmentSpawnpoint += new Vector3(20, 0, 0);
        _playerSpawnPoint = _activeSpawnPlatform.transform.position;
        _gameManager.SetPlayerSpawnpoint(new Vector3(_playerSpawnPoint.x, _playerSpawnPoint.y + 3, _playerSpawnPoint.z));
        if (PfirstLevel)
        {
            PickRandomSkybox();
            SpawnPlayer();
        }
        //adds all the Segements to the '_selectedPrefabs' list;
        for (int i = 0; i < PEasyRooms; i++)
        {
            _selectedPrefabs.Add(_EasySegments[Random.Range(0, _EasySegments.Length)]);
        }
        for (int i = 0; i < PmediumRooms; i++)
        {
            _selectedPrefabs.Add(_MediumSegments[Random.Range(0, _MediumSegments.Length)]);
        }
        for (int i = 0; i < PhardRooms; i++)
        {
            _selectedPrefabs.Add(_HardSegments[Random.Range(0, _HardSegments.Length)]);
        }

        ShuffleList(_selectedPrefabs);
        _activeLevels.Add(Instantiate(_EmptyLevel, Vector3.zero, Quaternion.identity));

        //spawns the segments
        for (int i = 0; i < _selectedPrefabs.Count;i++)
        {
            GameObject segment = Instantiate(_selectedPrefabs[i], _segmentSpawnpoint, Quaternion.identity);
            LevelSegment ls = segment.GetComponent<LevelSegment>();

            //coloring the segment
            for (int j = 0; j < ls.objectsToColor.Length; j++)
            {
                ls.objectsToColor[j].material = _LevelColors[currentColorIndex];
            }

            //updates the spawnpoint so the next segments spawns correctly
            _segmentSpawnpoint += new Vector3(ls.totalLength, ls.finsihedYOffset, ls.finsihedZOffset);
            if (_segmentSpawnpoint.y < _lowestPointInLevel)
            {
                _lowestPointInLevel = _segmentSpawnpoint.y;
            }
            segment.transform.SetParent(_activeLevels[_activeLevels.Count - 1].transform);
        }
         SpawnEndPlatform().transform.SetParent(_activeLevels[_activeLevels.Count - 1].transform);
         _activeSpawnPlatform.transform.SetParent(_activeLevels[_activeLevels.Count - 1].transform);
         _activeLevels[_activeLevels.Count - 1].name = "Level: " + _gameManager.GetCurrentLevel();

        if (_activeLevels.Count > 5)
        {
            DespawnLevel(_activeLevels[0]);
        }
        currentColorIndex++;
        if (currentColorIndex > _LevelColors.Length - 1)
        {
            currentColorIndex = 0;
        }
        return _lowestPointInLevel;
    }
    
    /// <summary>
    /// Despawns level
    public void DespawnLevel(GameObject level) 
    {
        _activeLevels.Remove(level);
        Destroy(level);
    }

    /// <summary>
    /// Instanstiates the spawn platform
    /// </summary>
    /// <returns>Returns the instantiated object</returns>
    private GameObject PlaceSpawnPlatForm()
    {
        return Instantiate(_SpawnPlatformPrefab, _segmentSpawnpoint, Quaternion.identity);
    }
    /// <summary>
    /// Instanstiates the end platform
    /// </summary>
    /// <returns>Returns the instantiated object</returns>
    private GameObject SpawnEndPlatform()
    {
        return Instantiate(_EndPlatformPrefab, _segmentSpawnpoint, Quaternion.identity);
    }
   
    /// <summary>
    /// Shuffles a list
    /// </summary>
    private void ShuffleList(List<GameObject> PlistToShuffle)
    {
        for (int i = 0; i < PlistToShuffle.Count - 1; i++)
        {
            GameObject temp = PlistToShuffle[i];
            int randomInt = Random.Range(i, PlistToShuffle.Count);
            PlistToShuffle[i] = PlistToShuffle[randomInt];
            PlistToShuffle[randomInt] = temp;
        }
    }
   /// <summary>
   /// Loads all resources (e.g level segments, skyboxes)
   /// </summary>
    public void LoadResources()
    {
        _SkyboxMaterials = Resources.LoadAll<Material>(_SkyboxMaterialsFolder);
        _EasySegments = Resources.LoadAll<GameObject>(_EasySegmentsFolder);
        _MediumSegments = Resources.LoadAll<GameObject>(_MediumSegmenetsFolder);
        _HardSegments = Resources.LoadAll<GameObject>(_HardSegmentsFolder);
    }
}
