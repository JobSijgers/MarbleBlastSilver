using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private string _TextToDisplay;
    [SerializeField] private TMP_Text _Text;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Text.text = _TextToDisplay;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Text.text = null;
        }
    }
    public void TutorialCompleted()
    {

    }
}
