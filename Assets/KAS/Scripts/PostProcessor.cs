﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessor : MonoBehaviour
{
    AudioSpectrum spectrum;
    //post processing profiler references
    public PostProcessingProfile myPost;
    public ColorGradingModel.Settings colorGrader;
    public GrainModel.Settings grainModel;

    public bool positiveColor, grainOn;
    public float globalLevelMin;
    public float colorChange;
    public float graintensity, luminance, sizeMult;

    //calibrate all the post processing values at start because these change outside playmode
    void Start()
    {
        spectrum = GetComponent<AudioSpectrum>();

        colorGrader = myPost.colorGrading.settings;

        colorGrader.basic.hueShift = 0;

        myPost.colorGrading.settings = colorGrader;

        grainModel = myPost.grain.settings;

        grainModel.intensity = 0;
        grainModel.luminanceContribution = 0;
        grainModel.size = 0.3f;

        myPost.grain.settings = grainModel;

        grainOn = true;
        positiveColor = true;
    }

    void Update()
    {
        if (positiveColor)
        {
            colorGrader.basic.hueShift = spectrum.MeanLevels[2] * colorChange;
        }
        else
        {
            colorGrader.basic.hueShift = spectrum.MeanLevels[2] * -colorChange;
        }

        myPost.colorGrading.settings = colorGrader;

        //grain effect responds to music
        if (grainOn)
        {
            grainModel.intensity = spectrum.MeanLevels[3] * graintensity;
            grainModel.luminanceContribution = spectrum.MeanLevels[4] * luminance;
            grainModel.size = spectrum.MeanLevels[5] * sizeMult;

            myPost.grain.settings = grainModel;
        }
        //reset grain off
        else
        {
            grainModel.intensity = 0;
            grainModel.luminanceContribution = 0;
            grainModel.size = 0.3f;

            myPost.grain.settings = grainModel;
        }

        //shift grain on/off
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            grainOn = !grainOn;
        }

        //switch color scale
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            positiveColor = !positiveColor;
        }
        //less smoke, higher level min
        if (Input.GetKey(KeyCode.DownArrow))
        {
            globalLevelMin += Time.deltaTime * 0.1f;
        }
        //more smoke, lower level min
        if (Input.GetKey(KeyCode.UpArrow))
        {
            globalLevelMin -= Time.deltaTime * 0.1f;
        }
        //increase color change var
        if (Input.GetKey(KeyCode.RightArrow))
        {
            colorChange += Time.deltaTime * 10f;
        }
        //decrease color change var
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            colorChange -= Time.deltaTime * 10f;
        }

    }

    
}

