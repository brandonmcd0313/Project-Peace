using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueUser : MonoBehaviour
{
  protected DialogueManager dialogueManager;
   protected ScriptableObject[] assets;
    protected string nameForDialogue;
   
    // Start is called before the first frame update
  protected  void Start()
    {
        //hook up to the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

  protected  void OnBeginDialogue()
    { 
        print("begin dialogue");
        //set the name for every assest to the name of this 
        foreach(ScriptableObject asset in assets)
        {
            switch (asset.GetType())
            {
                case Type t when t == typeof(DialogueAsset):
                    DialogueAsset dialogueAsset = (DialogueAsset)asset;
                dialogueAsset.PersonName = nameForDialogue;
                    break;
                case Type t when t == typeof(ChoicesAsset):
                    ChoicesAsset choicesAsset = (ChoicesAsset)asset;
                    choicesAsset.PersonName = nameForDialogue;
                    break;
                case Type t when t == typeof(ImpactTextAsset):
                    ImpactTextAsset impactTextAsset = (ImpactTextAsset)asset;
                    impactTextAsset.PersonName = nameForDialogue;
                    break;
                default:
                    break;
            }
        }
        dialogueManager.StartAssetSet(assets);
    }

}
