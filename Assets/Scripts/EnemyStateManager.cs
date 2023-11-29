using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateManager : MonoBehaviour
{
    string EnemyState = "EnemyState";
    string SceneOfAnger = "SceneOfAnger";
    EnemyController controllerInstance;


    public static EnemyStateManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PlayerPrefs.DeleteAll();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            //destroy this instance
            Destroy(gameObject);
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        controllerInstance = FindObjectOfType<EnemyController>();
        if (controllerInstance != null)
        {
            controllerInstance.OnPlayerAttackEnemy += EnableEnragement;
        }
        
        print("scene loaded");
        //check if player pref has been activated
        //check if player pref has been activated
        if (PlayerPrefs.GetInt(EnemyState) == 0)
        {
            print("enemy state is 0");
            //not activated do nothing
            return;
        }
        if (PlayerPrefs.GetString(SceneOfAnger) == SceneManager.GetActiveScene().name)
        {
            print("scene of anger is this scene");
            //reloaded scene or death
            //set enemy state to 0
            PlayerPrefs.SetInt(EnemyState, 0);
        }
        else //not enraged in this scene and not calm
        {
            print("enrage all enemies");
            //trigger the c# event to enrage all enemies
           Invoke("EnrageAllActiveEnemies", 0.15f);

        }
    }

    void EnrageAllActiveEnemies()
    {
        controllerInstance.OnPlayerAttackEnemy?.Invoke();
    }
    void EnableEnragement()
    {
        if(PlayerPrefs.GetInt(EnemyState) == 1)
        {
            return;
        }
        //set enemy state to 1
        PlayerPrefs.SetInt(EnemyState, 1);
        //set scene of anger to this scene
        PlayerPrefs.SetString(SceneOfAnger, SceneManager.GetActiveScene().name);
    }

    
}
