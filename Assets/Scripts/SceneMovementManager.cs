using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementManager : MonoBehaviour
{
    [TooltipAttribute("Requires both scenes to be set to 0")]
    [SerializeField] bool _assignDefaultSceneValues = true;

    [TooltipAttribute("Set to -1 to disable")]
    [SerializeField] int _nextSceneIndex;

    [TooltipAttribute("Set to -1 to disable")]
    [SerializeField] int _previousSceneIndex;

    [Space(5)]
    GameObject _player;
    SceneLocker _sceneLocker;
    // Start is called before the first frame update
    void Start()
    {
        if(_assignDefaultSceneValues)
        {
            if (_nextSceneIndex == 0)
            {
                _nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
            }
            if (_previousSceneIndex == 0)
            {
                _previousSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
            }
        }

        
        if (GetComponent<SceneLocker>() != null)
        {
            _sceneLocker = GetComponent<SceneLocker>();
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<PlayerSceneController>().OnPlayerExitScreenSpaceRight += LoadNextScene;
        _player.GetComponent<PlayerSceneController>().OnPlayerExitScreenSpaceLeft += LoadPreviousScene;

    }

    void LoadNextScene()
    {
        if (_sceneLocker != null)
        {
            if (_sceneLocker.IsLockedToTheRight && _sceneLocker.IsLocked)
            {
                return;
            }
        }
        //load the next scene
        if (_nextSceneIndex != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
        }
    }

    void LoadPreviousScene()
    {
        if (_sceneLocker != null)
        {
            if (!_sceneLocker.IsLockedToTheRight && _sceneLocker.IsLocked)
            {
                return;
            }
        }
        //load the previous scene
        if (_previousSceneIndex != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_previousSceneIndex);
        }

    }
}
