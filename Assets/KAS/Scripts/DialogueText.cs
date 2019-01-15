using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

//used to fake speaking
//[Serializable]
//public struct Alphabet
//{
//    public char letter;
//    public AudioClip spokenSound;
//}

public class DialogueText : MonoBehaviour
{
    //player refs
    GameObject player;
    MovePlayer mp;

    //text component and string array of its lines
    Text theText;
    public string[] textLines;

    //current and last lines
    public int currentLine;
    public int endAtLine;

    //typing vars
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;

    //wait between lines
    public float waitTime;

    //check this to start at start
    public bool enableAtStart;

    //audio stuff
    public AudioSource[] myVoices;
    public int currentVoice;
    public AudioClip[] mySounds;

    //for spoken alphabet
    public List<char> letters = new List<char>();
    public List<char> capitalLetters = new List<char>();
    public List<AudioClip> spokenSounds = new List<AudioClip>();

    void Start()
    {
        //grab refs
        player = GameObject.FindGameObjectWithTag("Player");
        mp = player.GetComponent<MovePlayer>();
        theText = GetComponent<Text>();

        if (textLines.Length == 0)
        {
            textLines = (theText.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length;
        }

        if (!enableAtStart)
        {
            theText.enabled = false;
        }
        else
        {
            EnableDialogue();
        }
    }

    void ProgressLine()
    {
        currentLine += 1;

        if (currentLine >= endAtLine)
        {
            DisableDialogue();
        }

        else
        {
            StartCoroutine(TextScroll(textLines[currentLine]));
        }
    }

    //Coroutine that types out each letter individually
    private IEnumerator TextScroll(string lineOfText) 
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            Speak(lineOfText[letter]);
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;

        yield return new WaitForSeconds(waitTime);

        ProgressLine();

    }

    public void EnableDialogue()
    {
        theText.enabled = true;

        StartCoroutine(TextScroll(textLines[currentLine]));
    }


    public void DisableDialogue()
    {
        theText.enabled = false;
    }

    //check through our alphabet of sounds and play corresponding character
    public void Speak(char letter)
    {
        //cycle through audioSources for voice
        if(currentVoice < myVoices.Length - 1)
        {
            currentVoice++;
        }
        else
        {
            currentVoice = 0;
        }

        //check in letters
        if (letters.Contains(letter))
        {
            int index = letters.IndexOf(letter);
            myVoices[currentVoice].clip = spokenSounds[index];
            myVoices[currentVoice].PlayOneShot(spokenSounds[index]);

        }
        //check in capital letters
        else if (capitalLetters.Contains(letter))
        {
            int index = capitalLetters.IndexOf(letter);
            myVoices[currentVoice].clip = spokenSounds[index];
            myVoices[currentVoice].PlayOneShot(spokenSounds[index]);

        }
        //punctuation or other stuff?
        else
        {
            PlaySound();
  
        }
    }

    //to play a sound
    public void PlaySound()
    {
        int randomSound = UnityEngine.Random.Range(0, mySounds.Length);
        myVoices[currentVoice].clip = mySounds[randomSound];
        myVoices[currentVoice].PlayOneShot(mySounds[randomSound]);
    }
}

