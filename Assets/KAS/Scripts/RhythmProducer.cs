using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmProducer : MonoBehaviour {
    public AudioClip[] basicBeats;
    public AudioSource beatSource;

    public bool playedAudio, showRhythm, changeRhythm;

    public int timeScale;

    public List<ParticleSystem> smokeParticles = new List<ParticleSystem>();

    public Animation animator;

    SmokeGenerator smokeGen;

    public int smokeCounter = 0;

    public void Awake()
    {
        SimpleClock.ThirtySecond += OnThirtySecond;
    }

    public void OnDestroy()
    {
        SimpleClock.ThirtySecond -= OnThirtySecond;
    }

    public void OnThirtySecond(BeatArgs e)
    {
        if (e.TickMask[TickValue.Measure])
        {
            // rhythm creation / beat visual
            changeRhythm = true;
        }

        //else if (e.TickMask[TickValue.Quarter])
        //{
        //    // rhythm creation / beat visual
        //    changeRhythm = true;
        //}

        switch (timeScale)
        {
            case 0:
                if (e.TickMask[TickValue.Measure])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case 1:
                if (e.TickMask[TickValue.Quarter])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case 2:
                if (e.TickMask[TickValue.Eighth])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case 3:
                if (e.TickMask[TickValue.Sixteenth])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case 4:
                if (e.TickMask[TickValue.ThirtySecond])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
        }

    }

    public void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            smokeParticles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }

        animator = GetComponent<Animation>();

        // randomize tempo var?
        beatSource = GetComponent<AudioSource>();

        RandomClip();

        RandomTempo();
    }

    // Update is called once per frame
    public void Update () {
        //may need to qualify this with and if statement in override
        AudioRhythm();
    }

    public void RandomTempo()
    {
        int randomTempo = Random.Range(0, 4);
        timeScale = randomTempo;
    }

    public void RandomClip()
    {
        int randomClip = Random.Range(0, basicBeats.Length);
        beatSource.clip = basicBeats[randomClip];
    }

    public void AudioRhythm()
    {
        if (!playedAudio)
        {
            SwitchTimeScale();
            playedAudio = true;
        }
        else
        {
            if (!beatSource.isPlaying)
            {
                if (changeRhythm)
                {
                    float randomChance = Random.Range(0, 100);

                    if(randomChance > 50)
                    {
                        RandomClip();
                        RandomTempo();
                    }
                    
                    changeRhythm = false;
                }

                SwitchTimeScale();
                playedAudio = false;

            }
        }

        //for showing smoke when a note is destined to play
        if (showRhythm)
        {
            animator.Play();
            smokeGen = smokeParticles[smokeCounter].GetComponent<SmokeGenerator>();
            smokeGen.SmokeIt();
            smokeParticles[smokeCounter].Play();
            showRhythm = false;

            if(smokeCounter < 2)
            {
                smokeCounter++;
            }
            else
            {
                smokeCounter = 0;
            }
           
        }
    }

    public void SwitchTimeScale()
    {
        switch (timeScale)
        {
            case 0:
                beatSource.PlayScheduled(SimpleClock.AtNextMeasure());
                break;
            case 1:
                beatSource.PlayScheduled(SimpleClock.AtNextQuarter());
                break;
            case 2:
                beatSource.PlayScheduled(SimpleClock.AtNextEighth());
                break;
            case 3:
                beatSource.PlayScheduled(SimpleClock.AtNextSixteenth());
                break;
            case 4:
                beatSource.PlayScheduled(SimpleClock.AtNextThirtySecond());
                break;
        }
    }
}
