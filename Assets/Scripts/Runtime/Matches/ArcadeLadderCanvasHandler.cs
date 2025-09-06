using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles transitions between arcade stages
public class ArcadeLadderCanvasHandler : MonoBehaviour
{
    public ArcadeLadderCameraHandler arcadeLadderCameraHandler;

    //Set by scene
    public CheckAudioStatus checkMusicPlaying;

    public PopulateArcadeLadderCanvas populator;

    private RectTransform playerNibRect;

    private float currentCharacterVertical;

    private void Start()
    {
        GameObject playerNib = transform.Find("PlayerNib").gameObject;

        populator = GetComponent<PopulateArcadeLadderCanvas>();
        playerNibRect = playerNib.GetComponent<RectTransform>();

        GameObject currentCharacterButton = null;

        //Assuming it's not the first stage
        if (currentCharacterVertical > 0)
        {
            currentCharacterButton = populator.characters[populator.arcadeRunData.currentArcadeStageIndex - 1];

            currentCharacterVertical = currentCharacterButton.GetComponent<RectTransform>().position.z;
        }
        else
        {
            currentCharacterButton = populator.characters[populator.arcadeRunData.currentArcadeStageIndex];
            currentCharacterVertical = currentCharacterButton.GetComponent<RectTransform>().position.z;

            playerNibRect.position = new Vector3(-145, 80, -500);
        }

        arcadeLadderCameraHandler.SetNewCameraPosition(currentCharacterButton.transform.position + new Vector3(0, 150, -200));
    }

    private void Update()
    {
        playerNibRect.position = Vector3.Lerp(playerNibRect.position, new Vector3(-145, 80, currentCharacterVertical), 2 * Time.deltaTime);
    }
}
