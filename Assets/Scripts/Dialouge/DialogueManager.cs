using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DialogueManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _dialogueText;
    [SerializeField] Image _textBox;
    [SerializeField] Button _choiceBox1, _choiceBox2;
    [SerializeField] TextMeshProUGUI _choiceText1, _choiceText2;

    
    public Action OnPlayerMakeChoice;
    
    KeyCode exitKey = KeyCode.Space;
    
    int currentDialogue;
    int currentAssetIndex;
    bool runningDialogueSet;

    string gotoDialogueName = "";

    void Start()
    {
        SetAllObjectsToEnabled(false);

        currentDialogue = 0;
        currentAssetIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(exitKey))
        {
            currentDialogue++;
        }
    }
    
    public void StartAssetSet(ScriptableObject[] assets)
    {
        currentAssetIndex = 0;
        StopAllCoroutines();
        EndDialogue();

    }
    
    IEnumerator GoThroughAssetSet(ScriptableObject[] assets)
    {
        foreach(ScriptableObject asset in assets)
        {
            if (gotoDialogueName == "")
            {

                StartAsset(asset);
                runningDialogueSet = true;
                //wait until no longer running dialogue set
                yield return new WaitUntil(() => runningDialogueSet == false);
            }
            else
            {
                //if not a dialogue
                if(asset.GetType() != typeof(DialogueAsset))
                {
                    //do not prompt this 
                }
                else
                {
                    DialogueAsset dialogueAsset = (DialogueAsset)asset;
                    if(dialogueAsset.OptionalDialogueAssetName == gotoDialogueName)
                    {
                        StartAsset(asset);
                        runningDialogueSet = true;
                        //wait until no longer running dialogue set
                        yield return new WaitUntil(() => runningDialogueSet == false);
                    }
                }
            }
        }
        EndDialogue();
    }
    
    public void StartAsset(ScriptableObject asset)
    {

        currentDialogue = 0;
        
        
        //check the type of asset
        switch (asset.GetType())
        {
            case Type t when t == typeof(DialogueAsset):
                DialogueAsset dialogueAsset = (DialogueAsset)asset;
                StartCoroutine(ShowDialogueSet(dialogueAsset));
                break;
            case Type t when t == typeof(ChoicesAsset):
                ChoicesAsset choicesAsset = (ChoicesAsset)asset;
                StartCoroutine(ShowChoices(choicesAsset));
                break;
            case Type t when t == typeof(ImpactTextAsset):
                ImpactTextAsset impactTextAsset = (ImpactTextAsset)asset;
                StartCoroutine(ShowImpactText(impactTextAsset));
                break;
            default:
                break;
        }
       
    }

    IEnumerator ShowDialogueSet(DialogueAsset asset)
    {
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(false);
        
        _nameText.text = " " + asset.PersonName + "...";
        string[] dialogueSet = asset.DialogueSet;

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

        _nameText.text = null;
        _dialogueText.text = null;

        runningDialogueSet = false;
    }

    IEnumerator ShowChoices(ChoicesAsset asset)
    {
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(true);

        _nameText.text = " " + asset.PersonName + "...";
        string dialoguePrompt = asset.DialoguePrompt;
        _dialogueText.text = dialoguePrompt;

        choice1GotoDialogueName = asset.Choice1GotoDialogueName;
       choice2GotoDialogueName = asset.Choice2GotoDialogueName;

        //wait until on player make choice is called
        yield return new WaitUntil(() => OnPlayerMakeChoice != null);

        choice1GotoDialogueName = "";
        choice2GotoDialogueName ="";

        _nameText.text = null;
        _dialogueText.text = null;
        SetAllChoicesToEnabled(false);
        runningDialogueSet = false;
    }

    IEnumerator ShowImpactText(ImpactTextAsset asset)
    {
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(false);

        _nameText.text = " " + asset.PersonName + "...";
        _dialogueText.text = null;
        GameObject textPrefab = asset.ImpactTextPrefab;

        //spawn the prefab at the center of the text box
        RectTransform textBoxRect = _textBox.GetComponent<RectTransform>();
        // Calculate the center position of the textBox in world space
        Camera mainCamera = Camera.main; 
        Vector3 centerPositionScreen = textBoxRect.position;
        Vector3 centerPositionWorld = mainCamera.ScreenToWorldPoint(centerPositionScreen);
        GameObject impactTextInstance = Instantiate(textPrefab, centerPositionWorld, Quaternion.identity);

        //wait until the current dialouge count increases
        int temp = currentDialogue;
        yield return new WaitUntil(() => currentDialogue != temp);

        _nameText.text = null;
        _dialogueText.text = null;
        Destroy(impactTextInstance);
        runningDialogueSet = false;

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

    string choice1GotoDialogueName, choice2GotoDialogueName;

    public void ButtonOne()
    {
        gotoDialogueName = choice1GotoDialogueName;
        OnPlayerMakeChoice?.Invoke();
    }

    public void ButtonTwo()
    {
        gotoDialogueName = choice2GotoDialogueName;
        OnPlayerMakeChoice?.Invoke();
    }
}



 