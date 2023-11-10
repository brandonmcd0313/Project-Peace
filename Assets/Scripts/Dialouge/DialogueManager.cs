using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;
using System;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _nameText, _dialogueText;
    [SerializeField] Image _textBox;
    [SerializeField] Button _choiceBox1, _choiceBox2;
    [SerializeField] TextMeshProUGUI _choiceText1, _choiceText2;

    public Action OnPlayerMakeChoice;
    KeyCode exitKey = KeyCode.Space;
    int currentDialogue;

    void Start()
    {
        SetAllObjectsToEnabled(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(exitKey))
        {
            currentDialogue++;
        }
    }
    public void StartDialogueSet(string[] dialogueSet, string name)
    {
        StopAllCoroutines();
        EndDialogue();
        currentDialogue = 0;
        
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(false);

        _nameText.text = name + "...";
        StartCoroutine(ShowDialogues(dialogueSet));
    }

    IEnumerator ShowDialogues(string[] dialogueSet)
    {
        while (currentDialogue < dialogueSet.Length)
        {
            _dialogueText.text = "";
            string text = dialogueSet[currentDialogue];
            print(text);
            foreach (char c in text)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(0.05f);
                //if player has skipped to next dialouge
                if (text != dialogueSet[currentDialogue])
                {
                    break;
                }

            }
            
            if (text == dialogueSet[currentDialogue])
            {
                //wait until the dialogue count is increased 

                int temp = currentDialogue;
                yield return new WaitUntil(() => currentDialogue != temp);
            }
            //player skipped ignore the wait

        }
        EndDialogue();
    }
    public void ShowChoice(string dialogue, string name, string choice1, string choice2)
    {
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(true);

        _nameText.text = name + "...";
        _dialogueText.text = dialogue;
        _choiceText1.text = choice1;
        _choiceText2.text = choice2;
    }
    public void EndDialogue()
    {
        _nameText.text = null;
        _dialogueText.text = null;
        SetAllChoicesToEnabled(false);
    }

    void SetAllObjectsToEnabled(bool value)
    {
        
            _nameText.enabled = value;
            _dialogueText.enabled = value;
            _textBox.enabled = value;
            _choiceBox1.enabled = value;
            _choiceBox2.enabled = value;
            _choiceText1.enabled = value;
            _choiceText2.enabled = value;
    }

    void SetAllChoicesToEnabled(bool value)
    {
            _choiceBox1.enabled = value;
            _choiceBox2.enabled = value;
            _choiceText1.enabled = value;
            _choiceText2.enabled = value;
    }
}



 