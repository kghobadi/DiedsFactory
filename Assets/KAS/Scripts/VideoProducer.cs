using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoProducer : MonoBehaviour
{

    //movement of screens back and forth
    //change the screens footage in relation to volume of x frequency range
    //array of video clips (these will be different for different prefabs)
    MovePlayer mp;
    AudioSpectrum spectrum;
    VideoClipList vidList;

    public int octaveBand;

    public float levelMin;

    private VideoPlayer vidPlayer;

    public List<VideoClip> naturalDisasters = new List<VideoClip>();
    //public List<VideoClip> spaceTravel = new List<VideoClip>();
    public bool staticDeath;

    public bool inOrOut;

    public bool hasReset;
    float resetTimer;
    public float resetMin, resetMax;

    public int direction;

    public float shiftSpeed;

    AudioSource screenAudio;


    void Start()
    {
        vidPlayer = GetComponent<VideoPlayer>();
        spectrum = GameObject.FindGameObjectWithTag("ScreenWall").GetComponent<AudioSpectrum>();
        vidList = spectrum.GetComponent<VideoClipList>();
        screenAudio = GetComponent<AudioSource>();
        Random.InitState(System.DateTime.Now.Millisecond);
        ChangeVidClip();
        StartCoroutine(WaitForStatic());
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>();
    }

    void Update()
    {
        if (!staticDeath)
        {
            CompareLevels();
            ScreenDepth();
        }


        if (!vidPlayer.isPlaying)
        {
            if (!screenAudio.isPlaying)
                screenAudio.Play();
        }
        else
        {
            if (screenAudio.isPlaying)
                screenAudio.Stop();
        }
    }

    //fluctuates depth of screens
    void ScreenDepth()
    {
        if (inOrOut && transform.localPosition.z < 64f)
        {
            transform.Translate(0, 0, spectrum.MeanLevels[octaveBand] * Time.deltaTime * shiftSpeed);
        }
        else if (!inOrOut && transform.localPosition.z > -24f)
        {
            transform.Translate(0, 0, -spectrum.MeanLevels[octaveBand] * Time.deltaTime * shiftSpeed);
        }
    }

    //has countdown and compares levels when necessary, then calls changeVidClip
    void CompareLevels()
    {
        Debug.Log("compared");
        if (hasReset)
        {
            resetTimer -= Time.deltaTime;

            if (resetTimer < 0)
            {
                hasReset = false;
            }
        }

        if (spectrum.MeanLevels[octaveBand] > levelMin)
        {
            if (!hasReset)
            {
                ChangeVidClip();
            }

        }
    }

    //call this to change the clip
    public void ChangeVidClip()
    {
        int randomClip = Random.Range(0, naturalDisasters.Count);

        //rerun until we find a clip that isn't being used
        if (vidList.clipsBeingUsed[randomClip])
        {
            ChangeVidClip();
        }
        //clip is not being used, so we switch it
        else
        {
            //uncheck last clip being used
            int index = naturalDisasters.IndexOf(vidPlayer.clip);
            vidList.clipsBeingUsed[index] = false;

            //set new clip and check it as being used
            vidPlayer.clip = naturalDisasters[randomClip];
            vidList.clipsBeingUsed[randomClip] = true;

            //play clip, switch depth movement
            vidPlayer.Play();
            inOrOut = !inOrOut;
            resetTimer = Random.Range(resetMin, resetMax);
            hasReset = true;

            //PlayStatic();
            Debug.Log("changed");
        }

    }

    //called at start
    //IEnumerator WaitForSpaceShips()
    //{
    //    yield return new WaitForSeconds(70);
    //    ascension = true;

    //    //reset vidlist
    //    for(int i = 0; i < vidList.clipsBeingUsed.Count; i++)
    //    {
    //        vidList.clipsBeingUsed[i] = false;
    //    }

    //    StartCoroutine(WaitForStatic());
    //}

    //called after ascension
    IEnumerator WaitForStatic()
    {
        yield return new WaitForSeconds(90);
        staticDeath = true;
        vidPlayer.Stop();
        vidPlayer.clip = null;
        StartCoroutine(WaitForStaticDeath());
    }

    IEnumerator WaitForStaticDeath()
    {
        yield return new WaitForSeconds(3);
        mp.staticDeath = true;
    }

}