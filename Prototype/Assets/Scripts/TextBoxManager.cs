﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    public Text theText;

    public TextAsset textFile;
    public Queue<string> textLines;
    public string eventTag;

    private bool typing;
    private string currentSentence;

    // Use this for initialization
    void Start()
    {
        typing = false;
        textLines = new Queue<string>();
        StartDialogue();
    }

    public void StartDialogue()
    {
        if (textFile != null)
        {   
            textBox.gameObject.SetActive(true);
            GameManager.ins.enableControl = false;

            foreach(string line in textFile.text.Split('\n'))
            {
                textLines.Enqueue(line);
            }

            DisplayNextLine();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
        if(textLines.Count == 0)
        {
            typing = false;
            EndDialogue();
            if(eventTag == "startGame")
                eventTag = "help";
            else
                eventTag = "";
            return;
        }

        if(typing)
        {
            theText.text = currentSentence;
            typing = false;
            StopAllCoroutines();
            return;
        }
        else
        {
            string sentence = textLines.Dequeue();
            currentSentence = sentence;
            theText.text = sentence;
            StopAllCoroutines();
            typing = true;
            StartCoroutine(TypeSentence(sentence));
        }
    }

    void EndDialogue()
    {
        GameManager.ins.enableControl = true;
        textBox.SetActive(false);
        StartEvent(eventTag);
    }

    public void StartEvent(string eventName)
    {
        switch(eventName)
        {
            case "startGame":
                textBox = GameObject.Find("Dialogue Container").transform.GetChild(0).gameObject;
                theText = textBox.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>();
                textFile = Resources.Load("Text/opening_text") as TextAsset;
                StartDialogue();
                break;
            case "zoomKnife":
                StartDialogueWithPath("Text/zoom_knife");
                break;
            case "selectKnife":
                StartDialogueWithPath("Text/select_knife");
                break;
            case "zoomLaptop":
                StartDialogueWithPath("Text/zoom_laptop");
                break;
            case "selectLaptop":
                StartDialogueWithPath("Text/select_laptop");
                break;
            case "enhanceLaptop":
                StartDialogueWithPath("Text/enhance_laptop");
                break;
            case "enhancedLaptop":
                StartDialogueWithPath("Text/enhanced_laptop");
                break;
            case "selectLaptopNoEnhance":
                StartDialogueWithPath("Text/select_laptop_noEnhance");
                break;
            case "selectPixelate":
                StartDialogueWithPath("Text/select_pixelate");
                break;
            case "selectBody":
                StartDialogueWithPath("Text/select_body");
                break;
            case "zoomBody":
                StartDialogueWithPath("Text/zoom_body");
                break;
            case "moreClues":
                StartDialogueWithPath("Text/more_clues");
                break;
            case "help":
                GameObject panel = GameObject.Find("Help Menu").transform.GetChild(0).gameObject;
                panel.SetActive(true);
                break;
            default:
                break;
        }
    }

    void StartDialogueWithPath(string path)
    {
        textFile = Resources.Load(path) as TextAsset;
        eventTag = "";
        StartDialogue();
    }

    IEnumerator TypeSentence(string sentence)
    {
        theText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            theText.text += letter;
            yield return null;
        }
        typing = false;
    }

    public void setText(TextAsset text_file)
    {
        textFile = text_file;
    }
}