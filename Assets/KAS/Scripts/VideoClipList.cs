using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoClipList : MonoBehaviour {

    public List<bool> clipsBeingUsed = new List<bool>();

    void Start()
    {
        for(int i = 0; i < clipsBeingUsed.Count; i++)
        {
            clipsBeingUsed[i] = false;
        }
    }
}
