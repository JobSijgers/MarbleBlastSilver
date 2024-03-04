using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : MonoBehaviour
{
    [SerializeField] private Color _NoclipActiveColor;
    [SerializeField] private Color _NoclipInactiveColor;
    [SerializeField] private Image _NoclipButtonImage;

    private GameManager _gameManager;
    private PlayerMovement _playerMovement;
    private bool _noclipEnabled = false;
    private void Start()
    {
        _gameManager = GameManager.Instance;   
    }

    public void ChanceNoclipMode()
    {
        if (_playerMovement == null)
        {
            _playerMovement = _gameManager.GetPlayerMovement();
        }
        _noclipEnabled = !_noclipEnabled;

        _playerMovement.SetPlayerNoclipMode(_noclipEnabled);

        if (_noclipEnabled)
        {
            _NoclipButtonImage.color = _NoclipActiveColor;
        }
        else
        {
            _NoclipButtonImage.color = _NoclipInactiveColor;
        }
    }

}
