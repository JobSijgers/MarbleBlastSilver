using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement _PlayerMovement;
    [SerializeField] private float _respawnBelowY; 

    private void Update()
    {
        if (_PlayerMovement.transform.position.y < _respawnBelowY ||
            Input.GetKeyDown(KeyCode.R))
        {
            _PlayerMovement.ResetVelocity();
            _PlayerMovement.transform.position = transform.position;
        }
    }
    public void TutorialCompleted()
    {
        SceneManager.LoadScene(0);
    }
}
