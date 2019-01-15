using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour {

    public bool hasActivated;
    public Animator[] myAnimators;
    public string stateName;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!hasActivated)
            {
                //if greater than 1
                if(myAnimators.Length > 1)
                {
                    for(int i = 0; i < myAnimators.Length; i++)
                    {
                        myAnimators[i].SetTrigger(stateName);
                    }
                }
                else
                {
                    myAnimators[0].SetTrigger(stateName);
                }
            }
        }
    }
}
