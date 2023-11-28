using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpecialDialogue : DialogueUser
{
    [SerializeField] string _name;
    [SerializeField] ScriptableObject[] _goodEndingAssets;
    [SerializeField] ScriptableObject[] _badEndingAssets;
    string EnemyState = "EnemyState";
    bool hasSpoken = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        name = _name;
        if(PlayerPrefs.GetInt(EnemyState) == 0)
        {
            assets = _goodEndingAssets;
        }
        else
        {
            assets = _badEndingAssets;
        }
        base.Start();
        dialogueManager.OnDialogueEnd += EndSpeaking;
    }

    private void FixedUpdate()
    {
        //when the player is close enough to the object, start speaking
        if (Vector3.Distance(transform.position, player.transform.position) < 5f && !hasSpoken)
        {
            StartSpeaking();
            hasSpoken = true;
        }
    }
    void StartSpeaking()
    {
        print("start speaking");
        OnBeginDialogue();
        //disable player movement
        player.GetComponent<PlayerController>().SetMovement(false);
    }

    void EndSpeaking()
    {
        //end of game
        //prompt the replay button
    }
}
