using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //this will also keep track if the player angered the enemies in a previous scene
    
    public Action OnPlayerAttackEnemy;

    private void Start()
    {
        //ignore collisons between enemies
        Physics2D.IgnoreLayerCollision(6, 6, true);
    }

}

