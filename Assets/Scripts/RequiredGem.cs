using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredGem : MonoBehaviour
{
    public LevelSegment RequiredFor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RequiredFor.RequiredGemCollected(this);
            Destroy(gameObject);
        }
    }
}
