using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script holds all deactivated game objects until player is near them

public class WorldManager : MonoBehaviour
{   //inactive object list for storing stuff we turn off
    public List<GameObject> allInactiveObjects = new List<GameObject>();
    public float activationDistance = 75f;

    //player variables
    GameObject player;
    MovePlayer mp;
    

    void Start()
    {
        //player refs
        player = GameObject.FindGameObjectWithTag("Player");
        mp = player.GetComponent<MovePlayer>();

        StoreDeactiveObjects();
    }

    void Update()
    {
        //if player moves, we call the deactivation functions 
        if (!mp.playerNavMove.isStopped)
        {
            StoreDeactiveObjects();
        }
        
    }

    void StoreDeactiveObjects()
    {
        //loop through all objects and check distances from player
        for (int i = 0; i < allInactiveObjects.Count; i++)
        {
            if (allInactiveObjects[i] != null)
            {
                float distanceFromPlayer = Vector3.Distance(allInactiveObjects[i].transform.position, player.transform.position);

                if (distanceFromPlayer < (activationDistance + 5))
                {
                    //set object active
                    allInactiveObjects[i].SetActive(true);
                    //remove from list
                    allInactiveObjects.Remove(allInactiveObjects[i]);
                    //move i back once to account for change in list index
                    i--;
                }
            }
            //obj is destroyed
            else
            {
                //remove from list
                allInactiveObjects.Remove(allInactiveObjects[i]);
                //move i back once to account for change in list index
                i--;
            }

        }
    }
}
