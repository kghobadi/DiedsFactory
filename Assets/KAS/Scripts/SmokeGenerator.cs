using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGenerator : MonoBehaviour {

    ParticleSystem smoker;
    public Material[] smokeFaces;

    //randomly assign one of the smoke materials to this particle system
	void Start () {
        smoker = GetComponent<ParticleSystem>();

        SmokeIt();

	}

    public void SmokeIt()
    {
        int randomSmoker = Random.Range(0, smokeFaces.Length);

        smoker.GetComponent<Renderer>().material = smokeFaces[randomSmoker];
    }
	
}
