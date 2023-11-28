using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : DialogueUser
{
    [SerializeField] string _name;
    [SerializeField] ScriptableObject[] _assets;

    // Start is called before the first frame update
    void Start()
    {
        name = _name;

        assets = _assets;
       

       base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void StartSpeaking()
    {
        print("start speaking");
        OnBeginDialogue();   
    }


}
