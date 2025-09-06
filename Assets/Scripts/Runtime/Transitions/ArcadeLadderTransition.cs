using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(100)]
public class ArcadeLadderTransition : MonoBehaviour
{
    public ArcadeLadderCanvasHandler arcadeLadderCanvasHandler;

    public SceneTransitionManager sceneTransitionManager;

    //Set by scene
    public CheckAudioStatus checkMusicPlaying;

    public SceneLoader sceneLoader;
    private FighterGameManager gameManager;

    public bool transitionBegun = false;

    public AsyncOperation matchLoaded;

    public int nameToIndex;

    //Unity has no way to load by name, so, have to convert to it this way, it sucks
    private int StageNameToIndex(string stageName)
    {
        switch (stageName)
        {
            case "Stage_Ryu_Waterfall":
                nameToIndex = 10;
                break;
            case "Stage_Ken_PathFromTheDojo":
                nameToIndex = 11;
                break;
            case "Stage_Cody_HugeBlock":
                nameToIndex = 12;
                break;
            case "Stage_Lee_GreatWallOfChina":
                nameToIndex = 13;
                break;
            case "Stage_Mike_MountRushmore":
                nameToIndex = 14;
                break;
            case "Stage_Joe_Trainyard":
                nameToIndex = 15;
                break;
        }

        return nameToIndex;
    }

    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        matchLoaded = sceneLoader.LoadSceneAsync("_gamemanager", LoadSceneMode.Additive);
    }
    private void LoadFighterEssentials()
    {
        gameManager.activeSceneIndex = StageNameToIndex(arcadeLadderCanvasHandler.populator.arcadeRunData.fighterArcadeModeData.stageName);

        print($"Stage index: {gameManager.activeSceneIndex}");

        gameManager.fighter1 = arcadeLadderCanvasHandler.populator.arcadeRunData.arcadeCharacter;
        gameManager.fighter2 = arcadeLadderCanvasHandler.populator.arcadeRunData.fighterArcadeModeData.currentOpponent.GetComponent<EntityScript>();

        arcadeLadderCanvasHandler.populator.arcadeRunData.arcadeCharacter = gameManager.fighter1;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<FighterGameManager>();

        sceneTransitionManager = GameObject.Find("TransitionCanvas").GetComponent<SceneTransitionManager>();
        arcadeLadderCanvasHandler = GameObject.FindObjectOfType<ArcadeLadderCanvasHandler>();

        LoadFighterEssentials();
    }

    private void Update()
    {
        if (!checkMusicPlaying.isPlayingAudio && !transitionBegun)
        {
            transitionBegun = true;

            sceneTransitionManager.PlaySceneTransitions("CurtainsIn", 0.5f);
        }

        if (transitionBegun && sceneTransitionManager.transitionComplete)
        {
            print("Let's get ready for the next battle");

            LoadMatch();
        }
    }

    //Removes current scene to load the current match
    public void LoadMatch()
    {
        gameManager.LoadStage();

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.UnloadSceneAsync(currentScene);

        //Gameplay preload scene
        sceneLoader.LoadSceneAsync("_arcadegameplaypreload", LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }
}
