using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public bool hasActivated;
    public DialogueText[] myDialogues;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!hasActivated)
            {
                //if greater than 1
                if(myDialogues.Length > 1)
                {
                    for(int i = 0; i < myDialogues.Length; i++)
                    {
                        myDialogues[i].EnableDialogue();
                    }
                }
                else
                {
                    myDialogues[0].EnableDialogue();
                }
            }
        }
    }
}
