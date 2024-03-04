using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinish : MonoBehaviour
{
    [SerializeField] private TutorialManager _TutorialManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _TutorialManager.TutorialCompleted();
        }
    }
}
