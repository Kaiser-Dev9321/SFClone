using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchLoader : MonoBehaviour
{
    private SelectCharacters selectCharacters;
    private SelectStage selectStage;
    private FighterGameManager gameManager;

    private AsyncOperation matchLoaded;

    private void Awake()
    {
        matchLoaded = SceneManager.LoadSceneAsync("_gamemanager", LoadSceneMode.Additive);
    }

    private void Start()
    {
        gameManager = FindObjectOfType<FighterGameManager>();

        //Used in events
        selectCharacters = FindObjectOfType<SelectPVPCharacters>();
        selectStage = FindObjectOfType<SelectStage>();
    }

    //Removes current scene to load the current match
    public void LoadMatch()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        gameManager.activeSceneIndex = selectStage.activeStage;

        gameManager.fighter1 = selectCharacters.activeFighter1.GetComponent<EntityScript>();
        gameManager.fighter2 = selectCharacters.activeFighter2.GetComponent<EntityScript>();

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
