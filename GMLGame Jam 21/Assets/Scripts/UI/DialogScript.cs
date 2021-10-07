using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScript : MonoBehaviour
{
    public Text dialogText;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public void Start()
    {
        StartCoroutine(Type());
    }

    public void Update()
    {
        if (dialogText.text == sentences[index])
        {
            NextSentence();
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            StartCoroutine(Wait());
            //dialogText.text = "";
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        dialogText.text = "";
    }
}