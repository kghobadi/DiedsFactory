using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidControls : MonoBehaviour {
    //actual vid player
    //public VideoPlayer vidPlayer;
    //public GameObject vidObject;
    //we will start with 8 -- if that doesnt work try 4 max
    public VideoPlayer[] vidPlayers;
    public GameObject[] vidObjects;
    public List<VideoPlayer> activeVidPlayers = new List<VideoPlayer>();
    bool clearing;

    //clip lists
    public List<VideoClip> caioClips = new List<VideoClip>();
    public List<VideoClip> alexClips = new List<VideoClip>();
    public List<VideoClip> mattClips = new List<VideoClip>();
    public List<VideoClip> ianClips = new List<VideoClip>();
    public List<VideoClip> naturalDisasters = new List<VideoClip>();
    public List<VideoClip> spaceShips = new List<VideoClip>();
    public List<VideoClip> randomClips = new List<VideoClip>();
    public List<VideoClip> theBoys = new List<VideoClip>();

    //clip counters
    public int caioCounter, alexCounter, mattCounter, ianCounter, disasterCounter, shipCounter, randomCounter, boysCounter;
    public float tranSpeed= 5;

    void Start () {
        //vidObject.SetActive(false);
	}
	
	void Update () {

        //This will now just clear all vid players
        //MOVE backwards through list of active vid players and turn them off one at a time
        //for turning on and off video object
        if (Input.GetKeyDown(KeyCode.Space) && !clearing)
        {
            StartCoroutine(ClearVidPlayers());
            
            //bool hasChanged = false;

            //if (vidObject.activeSelf && !hasChanged)
            //{
            //    vidObject.SetActive(false);
            //    hasChanged = true;
            //}
            //else if (!vidObject.activeSelf && !hasChanged)
            //{
            //    vidObject.SetActive(true);
            //    hasChanged = true;
            //}
        }

        //only allow change clips while active
        //control all vid players alpha val
        if (Input.GetKey(KeyCode.RightControl))
        {
            for (int i = 0; i < vidPlayers.Length; i++)
            {
                if (vidPlayers[i].isPlaying)
                {
                    vidPlayers[i].targetCameraAlpha += Time.deltaTime * tranSpeed * 2;
                }
                
            }
            
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            for (int i = 0; i < vidPlayers.Length; i++)
            {
                if (vidPlayers[i].isPlaying)
                {
                    vidPlayers[i].targetCameraAlpha -= Time.deltaTime * tranSpeed * 2;
                }
            }
        }

        //left hand
        //vid group A
        if (Input.GetKey(KeyCode.Q))
        {
            vidPlayers[0].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call caio clips
        if (Input.GetKeyDown(KeyCode.A))
        {
            CountActiveVids(0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            vidPlayers[0].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //vid group S
        if (Input.GetKey(KeyCode.W))
        {
            vidPlayers[1].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call alex clips
        if (Input.GetKeyDown(KeyCode.S))
        {
            CountActiveVids(1);
        }
        if (Input.GetKey(KeyCode.X))
        {
            vidPlayers[1].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }

        //vid group D
        if (Input.GetKey(KeyCode.E))
        {
            vidPlayers[2].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call matt clips
        if (Input.GetKeyDown(KeyCode.D))
        {
            CountActiveVids(2);
        }
        if (Input.GetKey(KeyCode.C))
        {
            vidPlayers[2].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //vid group F
        if (Input.GetKey(KeyCode.R))
        {
            vidPlayers[3].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call ian clips
        if (Input.GetKeyDown(KeyCode.F))
        {
            CountActiveVids(3);
        }
        if (Input.GetKey(KeyCode.V))
        {
            vidPlayers[3].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //right hand
        //vid group J
        if (Input.GetKey(KeyCode.U))
        {
            vidPlayers[4].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call disaster clips
        if (Input.GetKeyDown(KeyCode.J))
        {
            CountActiveVids(4);
        }
        //vid group J
        if (Input.GetKey(KeyCode.N))
        {
            vidPlayers[4].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //vid group K
        if (Input.GetKey(KeyCode.I))
        {
            vidPlayers[5].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call spaceships clips
        if (Input.GetKeyDown(KeyCode.K))
        {
            CountActiveVids(5);
        }
        if (Input.GetKey(KeyCode.M))
        {
            vidPlayers[5].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //vid group K
        if (Input.GetKey(KeyCode.O))
        {
            vidPlayers[6].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call random clips
        if (Input.GetKeyDown(KeyCode.L))
        {
            CountActiveVids(6);
        }
        if (Input.GetKey(KeyCode.Comma))
        {
            vidPlayers[6].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

        //vid group Semicolon
        if (Input.GetKey(KeyCode.P))
        {
            vidPlayers[7].targetCameraAlpha += Time.deltaTime * tranSpeed;
        }
        //call the boys clips
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            CountActiveVids(7);
        }
        if (Input.GetKey(KeyCode.Period))
        {
            vidPlayers[7].targetCameraAlpha -= Time.deltaTime * tranSpeed;
        }

    }

    //need to rework this so we have a list of active vid players and cycle out the oldest to replace with newests keypress
    void CountActiveVids(int vidToChange)
    {
        //less than 4 vids and this one isn't already active
        if (activeVidPlayers.Count < 4 && !activeVidPlayers.Contains(vidPlayers[vidToChange]))
        {
            ChangeClip(vidToChange);
            activeVidPlayers.Add(vidPlayers[vidToChange]);
        }
        //less than 4 vids and this one is already active
        else if (activeVidPlayers.Count < 4 && activeVidPlayers.Contains(vidPlayers[vidToChange]))
        {
            ChangeClip(vidToChange);
        }
        //already 4 vids and not in the list 
        else if (activeVidPlayers.Count == 4 && !activeVidPlayers.Contains(vidPlayers[vidToChange]))
        {
            //remove first vid player active in list, then add new one 
            activeVidPlayers[0].Stop();
            activeVidPlayers[0].clip = null;
            activeVidPlayers.Remove(activeVidPlayers[0]);

            //set new one and add to list
            ChangeClip(vidToChange);
            activeVidPlayers.Add(vidPlayers[vidToChange]);
        }
        //already 4 vids, but in the list
        else if (activeVidPlayers.Count == 4 && activeVidPlayers.Contains(vidPlayers[vidToChange]))
        {
            ChangeClip(vidToChange);
        }
    }

    //coroutine for deactivating video players
    IEnumerator ClearVidPlayers()
    {
        clearing = true;
        for (int i = 0; i < activeVidPlayers.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            activeVidPlayers[i].Stop();
            activeVidPlayers[i].clip = null;
            activeVidPlayers.Remove(activeVidPlayers[i]);
            i--;

        }
        clearing = false;
    }

    void ChangeClip(int vidplayer)
    {
        //sets their individual counters and picks from their clip lists
        switch (vidplayer)
        {
            //caio
            case 0:
                if (caioCounter < caioClips.Count - 1)
                {
                    caioCounter++;
                }
                else
                {
                    caioCounter = 0;
                }

                vidPlayers[vidplayer].clip = caioClips[caioCounter];
                break;
            //alex
            case 1:
                if (alexCounter < alexClips.Count - 1)
                {
                    alexCounter++;
                }
                else
                {
                    alexCounter = 0;
                }

                vidPlayers[vidplayer].clip = alexClips[alexCounter];
                break;
            //matt
            case 2:
                if (mattCounter < mattClips.Count - 1)
                {
                    mattCounter++;
                }
                else
                {
                    mattCounter = 0;
                }

                vidPlayers[vidplayer].clip = mattClips[mattCounter];
                break;
            //ian
            case 3:
                if (ianCounter < ianClips.Count - 1)
                {
                    ianCounter++;
                }
                else
                {
                    ianCounter = 0;
                }

                vidPlayers[vidplayer].clip = ianClips[ianCounter];
                break;
            //disasters
            case 4:
                if (disasterCounter < naturalDisasters.Count - 1)
                {
                    disasterCounter++;
                }
                else
                {
                    disasterCounter = 0;
                }

                vidPlayers[vidplayer].clip = naturalDisasters[disasterCounter];
                break;
            //spaceships
            case 5:
                if (shipCounter < spaceShips.Count - 1)
                {
                    shipCounter++;
                }
                else
                {
                    shipCounter = 0;
                }

                vidPlayers[vidplayer].clip = spaceShips[shipCounter];
                break;
            //random clips
            case 6:
                if (randomCounter < randomClips.Count - 1)
                {
                    randomCounter++;
                }
                else
                {
                    randomCounter = 0;
                }

                vidPlayers[vidplayer].clip = randomClips[randomCounter];
                break;
            //random clips
            case 7:
                if (boysCounter < theBoys.Count - 1)
                {
                    boysCounter++;
                }
                else
                {
                    boysCounter = 0;
                }

                vidPlayers[vidplayer].clip = theBoys[boysCounter];
                break;
        }

        vidPlayers[vidplayer].Play();
    }
}
