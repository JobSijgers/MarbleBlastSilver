using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enabled == false)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            gameManager.LevelCompleted();
            enabled = false;
        }


    }
}
