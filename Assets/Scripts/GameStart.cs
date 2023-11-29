using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("IsEnteringRight") != 1)
        {
            PlayerPrefs.DeleteAll();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
