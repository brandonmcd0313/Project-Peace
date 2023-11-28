using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChoicesAsset : ScriptableObject
{
    public string PersonName;
    
    [TextArea]
    public string DialoguePrompt;

    [TextArea]
    public string Choice1, Choice2;


    public string Choice1GotoDialogueName, Choice2GotoDialogueName;
}
