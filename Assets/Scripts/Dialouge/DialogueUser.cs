using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public abstract class DialogueUser : MonoBehaviour
{
  protected DialogueManager dialogueManager;
   protected ScriptableObject[] assets;
    protected string nameForDialogue;
   
    // Start is called before the first frame update
  protected void Start()
    {
        //hook up to the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>();
        //for each scriptable object check if PersonName is "", if it is set it to the name of this object
        foreach (ScriptableObject asset in assets)
        {
            switch (asset.GetType())
            {
                case System.Type t when t == typeof(DialogueAsset):
                    DialogueAsset dialogueAsset = (DialogueAsset)asset;
                    if (dialogueAsset.PersonName == "")
                    {
                        dialogueAsset.PersonName = name;
                    }
                    break;
                case System.Type t when t == typeof(ChoicesAsset):
                    ChoicesAsset choicesAsset = (ChoicesAsset)asset;
                    if (choicesAsset.PersonName == "")
                    {
                        choicesAsset.PersonName = name;
                    }
                    break;
                case System.Type t when t == typeof(ImpactTextAsset):
                    ImpactTextAsset impactTextAsset = (ImpactTextAsset)asset;
                    if (impactTextAsset.PersonName == "")
                    {
                        impactTextAsset.PersonName = name;
                    }
                    break;
                default:
                    break;
            }
        }
    }

  protected  void OnBeginDialogue()
    { 
        print("begin dialogue");
        dialogueManager.StartAssetSet(assets);
    }

}
