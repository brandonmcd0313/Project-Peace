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
    [SerializeField] AudioClip talking;


    public Action OnDialogueEnd;
    bool hasPlayerMadeChoice;
    
    KeyCode exitKey = KeyCode.Space;
    
    int currentDialogue;
    bool isRunningDialogueSet;

    string gotoDialogueName = "";

    void Start()
    {
        SetAllObjectsToEnabled(false);

        currentDialogue = 0;
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
        print("starting asset set");
        StopAllCoroutines();
        EndDialogue();
        StartCoroutine(GoThroughAssetSet(assets));
    }
    
    IEnumerator GoThroughAssetSet(ScriptableObject[] assets)
    {
        foreach(ScriptableObject asset in assets)
        {
           
            //check if the asset is a dialogue
            if (asset.GetType() == typeof(DialogueAsset))
            {
                DialogueAsset dialogueAsset = (DialogueAsset)asset;
                string toCheck = dialogueAsset.OptionalDialogueAssetName;
                //run it
                if (gotoDialogueName == "" && choice1GotoDialogueName == "" && choice2GotoDialogueName == "")
                {
                    StartAsset(asset);
                    print("Started asset");
                    isRunningDialogueSet = true;
                    //wait until no longer running dialogue set
                    yield return new WaitUntil(() => isRunningDialogueSet == false);
                }
                else if(toCheck == gotoDialogueName)
                {
                    gotoDialogueName = "";
                    StartAsset(asset);
                    isRunningDialogueSet = true;
                    //wait until no longer running dialogue set
                    yield return new WaitUntil(() => isRunningDialogueSet == false);
                }
                else
                {
                    if (toCheck == choice1GotoDialogueName || toCheck == choice2GotoDialogueName)
                    {
                        //it was the second choice. do NOT prompt
                        
                    }
                }

                //reset the appropriate choice name
                if (toCheck == choice1GotoDialogueName)
                {
                    choice1GotoDialogueName = "";
                }
                else if (toCheck == choice2GotoDialogueName)
                {
                    choice2GotoDialogueName = "";
                }

            }
            else
            {
                //if not a dialogue, run it
                StartAsset(asset);
                isRunningDialogueSet = true;
                //wait until no longer running dialogue set
                yield return new WaitUntil(() => isRunningDialogueSet == false);

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
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            audioSource.PlayOneShot(talking);
            foreach (char c in text)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(0.05f);
                //if player has skipped to next dialouge
                if(currentDialogue >= dialogueSet.Length)
                {
                    break;
                }
                else if (text != dialogueSet[currentDialogue])
                {
                    break;
                }

            }
            //player skipped ignore the wait
            if (currentDialogue >= dialogueSet.Length)
            {
                audioSource.Stop();
                break;
            }
               else if (text == dialogueSet[currentDialogue])
            {
                //wait until the dialogue count is increased 
                audioSource.Stop();
                int temp = currentDialogue;
                yield return new WaitUntil(() => currentDialogue != temp);
            }
   

        }

        _nameText.text = null;
        _dialogueText.text = null;

        isRunningDialogueSet = false;
    }

    IEnumerator ShowChoices(ChoicesAsset asset)
    {
        choice1GotoDialogueName = "";
        choice2GotoDialogueName = "";

        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(false);
      


        _nameText.text = " " + asset.PersonName + "...";
        _dialogueText.text = "";
        string text = asset.DialoguePrompt;
        foreach (char c in text)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);

        }
        SetAllChoicesToEnabled(true);
        //set buttons to the choices
        _choiceText1.text = asset.Choice1;
        _choiceText2.text = asset.Choice2;

        choice1GotoDialogueName = asset.Choice1GotoDialogueName;
       choice2GotoDialogueName = asset.Choice2GotoDialogueName;

        //wait until on player make choice is called
        yield return new WaitUntil(() => hasPlayerMadeChoice == true);
        hasPlayerMadeChoice = false;



        _nameText.text = null;
        _dialogueText.text = null;
        SetAllChoicesToEnabled(false);
        isRunningDialogueSet = false;
    }

    IEnumerator ShowImpactText(ImpactTextAsset asset)
    {
        SetAllObjectsToEnabled(true);
        SetAllChoicesToEnabled(false);

        _nameText.text = " " + asset.PersonName + "...";
        _dialogueText.text = null;
        GameObject textPrefab = asset.ImpactTextPrefab;

        //spawn the prefab at the center of the text box
        /*
        RectTransform textBoxRect = _textBox.GetComponent<RectTransform>();
        // Calculate the center position of the textBox in world space
        Camera mainCamera = Camera.main; 
        Vector3 centerPositionScreen = textBoxRect.position;
        Vector3 centerPositionWorld = mainCamera.ScreenToWorldPoint(centerPositionScreen);
        GameObject impactTextInstance = Instantiate(textPrefab, centerPositionWorld, Quaternion.identity);
        */
        GameObject impactTextInstance = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
        //play the audiclip using the audiosource on main camera
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(asset.ImpactTextSound);
        //wait until the current dialouge count increases
        int temp = currentDialogue;
        //screw it no escape
        yield return new WaitForSeconds(20000000f);
        /*
        yield return new WaitUntil(() => currentDialogue != temp);

        _nameText.text = null;
        _dialogueText.text = null;
        Destroy(impactTextInstance);
        isRunningDialogueSet = false;
        */

    }


    public void EndDialogue()
    {
        _nameText.text = null;
        _dialogueText.text = null;
        SetAllObjectsToEnabled(false);
        SetAllChoicesToEnabled(false);
        OnDialogueEnd.Invoke();

    }

    void SetAllObjectsToEnabled(bool value)
    {
        
            _nameText.enabled = value;
            _dialogueText.enabled = value;
            _textBox.enabled = value;
        if(_choiceBox1 == null)
        {
            return;
        }
        _choiceBox1.gameObject.SetActive(value);
        _choiceBox2.gameObject.SetActive(value);
        _choiceText1.enabled = value;
            _choiceText2.enabled = value;
    }

    void SetAllChoicesToEnabled(bool value)
    {
        if (_choiceBox1 == null)
        {
            return;
        }
        _choiceBox1.gameObject.SetActive(value);
        _choiceBox2.gameObject.SetActive(value);
        _choiceText1.enabled = value;
            _choiceText2.enabled = value;
    }

    string choice1GotoDialogueName = "", choice2GotoDialogueName = "";

    public void ButtonOne()
    {
        print("button one");
        gotoDialogueName = choice1GotoDialogueName;
        hasPlayerMadeChoice = true;
    }

    public void ButtonTwo()
    {
        print("button two");
        gotoDialogueName = choice2GotoDialogueName;
        hasPlayerMadeChoice = true;
    }
}



 