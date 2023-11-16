using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueUser : MonoBehaviour
{
    [SerializeField] protected DialogueManager _dialogueManager;
    [SerializeField] protected ScriptableObject[] Assests;
   
    // Start is called before the first frame update
    void Start()
    {
        //hook up to the dialogue manager
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnBeginDialogue()
    { 
    }

}
