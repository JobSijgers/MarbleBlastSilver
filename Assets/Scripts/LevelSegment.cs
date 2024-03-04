using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    public MeshRenderer[] objectsToColor;
    public float totalLength;
    public float finsihedYOffset;
    public float finsihedZOffset;

    public bool segmentReqGems;
    [HideInInspector] public List<RequiredGem> requiredGems;
    [HideInInspector] public GameObject barrier;

    public void FetchRequiredItems()
    {                
        RequiredGem[] requiredGems = GetComponentsInChildren<RequiredGem>();
        this.requiredGems.Clear();
        for (int i = 0; i < requiredGems.Length; i++)
        {
            this.requiredGems.Add(requiredGems[i]);
            requiredGems[i].RequiredFor = this;
        }
    }
    public void RequiredGemCollected(RequiredGem gem)
    {
        requiredGems.Remove(gem);
        Debug.Log(requiredGems.Count);
        if (requiredGems.Count <= 0)
        {
            Destroy(barrier);
        }
    }
}
                                                                           