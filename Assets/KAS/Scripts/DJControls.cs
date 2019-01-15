using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DJControls : MonoBehaviour {
    public AudioClip[] songs;
    AudioSource speakers;

    public int songCounter = 0;

	void Start () {
        Cursor.visible = false;

        speakers = GetComponent<AudioSource>();
        speakers.loop = true;
        speakers.clip = songs[0];
        speakers.Play();
	}

	void Update () {
        //cycle through songs up or down
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchSong(false);
        }
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            SwitchSong(true);
        }

        //quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //restart game
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SceneManager.LoadScene(0);
        }
    }

    void SwitchSong(bool upOrDown)
    {
        if (upOrDown)
        {
            if (songCounter < songs.Length - 1)
            {
                songCounter++;
            }
            else
            {
                songCounter = 0;
            }
        }
        else
        {
            if (songCounter > 0)
            {
                songCounter--;
            }
            else
            {
                songCounter = songs.Length - 1;
            }
        }

        speakers.Stop();
        speakers.clip = songs[songCounter];
        speakers.Play();
    }
}
