using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLocker : MonoBehaviour
{
    public bool IsLocked;
    public bool IsLockedToTheRight;
    // Start is called before the first frame update
    void Start()
    {
        IsLocked = true;
    }


    void UnlockScene()
    {
        IsLocked = false;
    }


}