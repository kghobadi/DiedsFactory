using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PostProcessing;

public class MovePlayer : MonoBehaviour {

    public NavMeshAgent playerNavMove;

    public Transform movementPointHolder;
    public Transform[] movementPoints, crashPoints;

    public int currentDest = 0, finishDest, currentCrashPoint = 0;

    public GameObject airship, crashingSmokes;
    public float moveSpeed, fallSpeed, crashSpeed;

    public AudioSource planeSource;

    float smokeSpawnTimer;
    public float smokeSpawnTotal = 1;

    SimpleClock clock;
    public float normalBPM, fastBPM;

    public bool hasCrashed, lerpingFOV, increasingFOV;
    public float desiredFOV, lerpSpeed;

    Camera myCam;

    public Animator[] characters;
    public DialogueText[] dialogues;

    //post processing profiler references
    public PostProcessingProfile myPost;
    public ColorGradingModel.Settings colorGrader;
    public bool staticDeath;
    public float staticFallSpeed = 3f;
    

    void Start () {
        //pp stuff
        colorGrader = myPost.colorGrading.settings;

        colorGrader.basic.hueShift = 0;

        myPost.colorGrading.settings = colorGrader;

        //set our movement points
        movementPoints = new Transform[movementPointHolder.childCount];
        for (int i =0; i < movementPointHolder.childCount; i++)
        {
            movementPoints[i] = movementPointHolder.transform.GetChild(i);
        }
        finishDest = movementPoints.Length - 1;

        playerNavMove = GetComponent<NavMeshAgent>();
        planeSource = airship.GetComponent<AudioSource>();
        planeSource.Play();
        SetDestination();
        smokeSpawnTimer = smokeSpawnTotal;
        clock = GameObject.FindGameObjectWithTag("SimpleClock").GetComponent<SimpleClock>();
        myCam = Camera.main;
	}
	
	void Update () {

        if(Vector3.Distance(transform.position, movementPoints[currentDest].position) > 10f)
        {
            //move when mouse down
            if (Input.GetMouseButton(0))
            {
                playerNavMove.isStopped = false;

                if (!hasCrashed)
                {
                    Time.timeScale = 0.5f;
                    ShipsFall();
                    if (!planeSource.isPlaying)
                    {
                        LerpFOV(90, 5);
                        planeSource.UnPause();
                        clock.SetBPM(fastBPM);
                        CharacterSpeeds(50);
                        DialogueSpeeds(0.00000001f);
                    }
                }
            }

            //stay stopped
            else
            {
                playerNavMove.isStopped = true;
                
                if (!hasCrashed)
                {
                  
                    Time.timeScale = 0.1f;
                    

                    if (planeSource.isPlaying)
                    {
                        LerpFOV(60, 5);
                        planeSource.Pause();
                        clock.SetBPM(normalBPM);
                        CharacterSpeeds(10);
                        DialogueSpeeds(0.005f);
                    }
                }
                else
                {
                    CharacterSpeeds(10);
                    DialogueSpeeds(0.005f);
                }
            }
        }
        else
        {
            if(currentDest < finishDest)
            {
                currentDest++;
                SetDestination();
            }
        }

        if (staticDeath)
        {
            staticFallSpeed += 0.5f;
            transform.Translate(0, -staticFallSpeed, 0);
        }

        //lerps fov 
        if (lerpingFOV)
        {
            myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, desiredFOV, Time.deltaTime * lerpSpeed);

            //when to stop depends on if increasing or decreasing
            if (increasingFOV)
            {
                if (myCam.fieldOfView > desiredFOV - 0.1f)
                {
                    lerpingFOV = false;
                }
            }
            else
            {
                if (myCam.fieldOfView < desiredFOV + 0.1f)
                {
                    lerpingFOV = false;
                }
            }
          
        }
	}

    //called to set cam to specific new fov
    public void LerpFOV(float fov, float speed)
    {
        desiredFOV = fov;
        if(desiredFOV > myCam.fieldOfView)
        {
            increasingFOV = true;
        }
        else
        {
            increasingFOV = false;
        }
        lerpSpeed = speed;
        lerpingFOV = true;
    }

    void CharacterSpeeds(float speed)
    {
        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].speed = speed;
        }
    }

    void DialogueSpeeds(float speed)
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i].typeSpeed = speed;
        }
    }

    //called when player arrives at desired movement point
    void SetDestination()
    {
        playerNavMove.SetDestination(movementPoints[currentDest].position);

        if(currentDest == finishDest)
        {
            LerpFOV(65, 1);
            myCam.GetComponent<camMouseLook>().isActive = false;
            transform.localEulerAngles = new Vector3(0, -135, 0);
            myCam.transform.localEulerAngles = new Vector3(-25, 3.69f, 0);
        }

       

        //when you arrive at the center of the factory room, start cinema
        //perhaps lock camera on view of the 9 TVs
        //need more footage and to tune their symmetry with audioSpectrum
        //dig into those post processing effects
        //play with arrangement and # of tvs in relation to octave banding
    }


    //called only while player is moving
    void ShipsFall()
    {
        //while falling through air
        if( currentCrashPoint < 3)
        {
            //randomly shake while falling
            float randomTranslateX = Random.Range(-1f, 1f);
            float randomTranslateZ = Random.Range(-1f, 1f);

            airship.transform.Translate(randomTranslateX, 0, randomTranslateZ);

            //spin plane around z axis
            float randomRotate = Random.Range(0f, 5f);

            airship.transform.Rotate(0, 0, randomRotate);

            moveSpeed = fallSpeed;
        }
        //hitting the ground
        else if(currentCrashPoint == 3)
        {
            airship.transform.localEulerAngles += new Vector3(0.5f, 0, 0);

            moveSpeed = crashSpeed;
        }

        //bouncing up
        else if(currentCrashPoint == 4)
        {
           
        }

        //always move towards next crash point at fallSpeed
        if (Vector3.Distance(airship.transform.position, crashPoints[currentCrashPoint].position) > 0.25f)
        {
            airship.transform.position = Vector3.MoveTowards(airship.transform.position, crashPoints[currentCrashPoint].position, moveSpeed * Time.deltaTime);
        }
        else
        {
            currentCrashPoint++;

            //activate tiny flame particles
            if(currentCrashPoint == 2)
            {

            }

            //reset rotation before the crash
            if(currentCrashPoint == 3)
            {
                airship.transform.localEulerAngles = new Vector3(airship.transform.localEulerAngles.x, airship.transform.localEulerAngles.y, 0f);

                //activate dirt effect on terrain
            }

            //activate more flame particles
            if (currentCrashPoint == 4)
            {
                //activate dirt effect on terrain
            }

            //activate final flame particles
            if (currentCrashPoint == 5)
            {
                //activate dirt effect on terrain
            }

            //activate final flame particles
            if (currentCrashPoint == 5)
            {
                //activate dirt effect on terrain
            }

            //final chance to add effects
            if (currentCrashPoint == 6)
            {
                
            }

            //activate final flame particles
            if (currentCrashPoint == 7)
            {
                hasCrashed = true;
                LerpFOV(100, 3);
                playerNavMove.speed = 30;
                Time.timeScale = 1;
                CharacterSpeeds(1);
            }
        }

        //spawn smoke clouds at smokeSpawnTotal interval
        smokeSpawnTimer -= Time.deltaTime;

        if(smokeSpawnTimer < 0)
        {
            GameObject smokeClone = Instantiate(crashingSmokes, airship.transform.position, Quaternion.identity);

            if(currentCrashPoint < 5)
            {
                smokeClone.GetComponent<WaitToDestroy>().destroy = true;
            }

            smokeSpawnTimer = smokeSpawnTotal;
        }
    }

  
}
