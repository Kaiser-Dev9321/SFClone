using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_PostMatchCanvas : MonoBehaviour
{
    private WinQuotesManager winQuotesManager;
    public TextMeshProUGUI postMatchText;

    public AudioSource music_PostMatch;

    public ArcadeRunData arcadeRunData;
    public SceneLoader sceneLoader;

    private void Awake()
    {
        winQuotesManager = FindObjectOfType<WinQuotesManager>();
        arcadeRunData = FindObjectOfType<ArcadeRunData>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Start()
    {
        postMatchText.text = winQuotesManager.winQuoteToUse.winQuote;
    }

    private void Update()
    {
        if (!music_PostMatch.isPlaying)
        {
            print($"<size=19>NEXT ARCADE OPPONENT</size>, stage name: {arcadeRunData.fighterArcadeModeData.stageName}");

            SceneManager.UnloadSceneAsync("Stage_PostMatchStage");
            sceneLoader.LoadScene("ArcadeLadderScene");
        }
    }
}
