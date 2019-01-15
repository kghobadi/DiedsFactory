using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryListener : MonoBehaviour {
    AudioSpectrum spectrum;
    PostProcessor ppTime;
    public int octaveBand;
    public float levelMin;

    public bool showRhythm;

    public List<ParticleSystem> smokeParticles = new List<ParticleSystem>();

    public Animation animator;

    SmokeGenerator smokeGen;

    public int smokeCounter = 0;

    public bool hasReset;
    float resetTimer;
    public float resetMin, resetMax;

    void Start () {
        spectrum = GameObject.FindGameObjectWithTag("ScreenWall").GetComponent<AudioSpectrum>();
        ppTime = spectrum.GetComponent<PostProcessor>();
        levelMin = ppTime.globalLevelMin;

        for (int i = 0; i < 3; i++)
        {
            smokeParticles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }

        animator = GetComponent<Animation>();

        //octaveBand = Random.Range(0, 9);
    }
	

	void Update () {
        CompareLevels();

        levelMin = ppTime.globalLevelMin;
        //for showing smoke when a note is destined to play
        if (showRhythm)
        {
            animator.Play();
            smokeGen = smokeParticles[smokeCounter].GetComponent<SmokeGenerator>();
            smokeGen.SmokeIt();
            smokeParticles[smokeCounter].Play();
            showRhythm = false;

            resetTimer = Random.Range(resetMin, resetMax);
            hasReset = true;
            


            if (smokeCounter < 2)
            {
                smokeCounter++;
            }
            else
            {
                smokeCounter = 0;
            }

        }

      
    }

    //has countdown and compares levels when necessary, then calls changeVidClip
    void CompareLevels()
    {
        //Debug.Log("compared");
        if (hasReset)
        {
            resetTimer -= Time.deltaTime;

            if (resetTimer < 0)
            {
                hasReset = false;
            }
        }

        if (spectrum.MeanLevels[octaveBand] > levelMin )
        {
            if (!hasReset)
            {
                showRhythm = true;
            }
        }
    }
}
