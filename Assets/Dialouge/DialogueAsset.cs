using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    
    public string OptionalDialogueAssetName;

    public string PersonName;
    
    [TextArea]
    public string[] DialogueSet;
}
