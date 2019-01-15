using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject player;

    public float currentDistance, openDistance;

    public bool open;

    Animator doorAnimator;

    AudioSource doorAudio;
    public AudioClip doorOpen, doorClose;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorAnimator = GetComponent<Animator>();
        doorAudio = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        currentDistance = Vector3.Distance(player.transform.position, transform.position);

        //if player is near, set door to open and play a sound
        if (!open)
        {
            if (currentDistance < openDistance)
            {
                doorAnimator.SetBool("open", true);
                open = true;
                PlaySound(doorOpen);
            }
        }
       
        //if player has walked away, set door to close and play a sound
        else if (open)
        {
            if(currentDistance > openDistance + 5)
            {
                doorAnimator.SetBool("open", false);
                open = false;
                PlaySound(doorClose);
            }
          
        }
    }

    void PlaySound(AudioClip sound)
    {
        doorAudio.PlayOneShot(sound);
    }
}
