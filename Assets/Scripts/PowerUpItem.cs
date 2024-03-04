using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [SerializeField] private EPowerUpType type;
    private PowerUpManager powerUpManager;
    private void Start()
    {
        powerUpManager = PowerUpManager.instance;   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpManager.PowerUpCollected(type);
            powerUpManager.StartCoroutine(powerUpManager.RespawnPowerup(gameObject, 5f));
        }
    }
}
