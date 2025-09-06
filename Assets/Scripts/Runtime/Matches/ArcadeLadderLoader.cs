using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeLadderLoader : MonoBehaviour
{
    private SceneLoader sceneLoader;

    public ArcadeRunData arcadeRunData;

    public SelectCharacters selectArcadeCharacter;

    public AsyncOperation matchLoaded;

    public int nameToIndex;

    private void Awake()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    private void Start()
    {
        arcadeRunData = GameObject.Find("ArcadeRunData").GetComponent<ArcadeRunData>();
    }

    public void LoadNewArcadeModeData(ArcadeModeData newArcadeModeData)
    {
        arcadeRunData.fighterArcadeModeData = newArcadeModeData;
        arcadeRunData.currentOpponentEntity = newArcadeModeData.currentOpponent.GetComponent<EntityScript>();
    }

    public void StartArcadeRun()
    {
        arcadeRunData.arcadeCharacter = selectArcadeCharacter.activeFighter1.GetComponent<EntityScript>();

        sceneLoader.LoadScene("ArcadeLadderScene");
    }
}
