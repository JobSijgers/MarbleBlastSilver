using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPowerUpType
{
    speedBoost
}
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance { get; private set; }
    private PlayerMovement playerMovement;
    private void Awake()
    {
        instance = this;
    }
    public void SetPlayerMovement(PlayerMovement movement)
    {
        playerMovement = movement;
    }
    /// <summary>
    /// Registers a powerup collection
    /// </summary>
    /// <param name="type">type of powerup that has been collected</param>
    public void PowerUpCollected(EPowerUpType type)
    {
        switch (type)
        {
            case EPowerUpType.speedBoost:
                {
                    playerMovement.AddForceInPlayerDirection(100);
                    break;
                }
        }
    }

    /// <summary>
    /// Respawns powerup
    /// </summary>
    /// <param name="PgameObject">Gameobject to respawn</param>
    /// <param name="Pdelay">delay to respawn</param>
    /// <returns></returns>
    public IEnumerator RespawnPowerup(GameObject PgameObject, float Pdelay)
    {
        PgameObject.SetActive(false);
        yield return new WaitForSeconds(Pdelay);
        PgameObject.SetActive(true);
    }
}
