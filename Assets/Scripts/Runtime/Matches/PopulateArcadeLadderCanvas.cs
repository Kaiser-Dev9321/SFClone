using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-50)]
public class PopulateArcadeLadderCanvas : MonoBehaviour
{
    public ArcadeRunData arcadeRunData;

    public ArcadeModeData[] arcadeStages = new ArcadeModeData[8];

    public GameObject[] characters = new GameObject[8];

    private RectTransform canvasTransform;

    private void PreloadAllArcadeStages()
    {
        //Clear character prefabs
        Destroy(transform.GetChild(0).gameObject);

        //TODO: ArcadeModeData will be renamed
        for (int i = 0; i < 8; i++)
        {
            arcadeStages[i] = Resources.Load<ArcadeModeData>($"Data/ArcadeModeData/{arcadeRunData.arcadeCharacter.fighterName}/ArcadeModeData_{arcadeRunData.arcadeCharacter.fighterName}_Stage{i + 1}");

            GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Canvases/ArcadeLadder/Character");

            GameObject characterObj = Instantiate(characterPrefab, canvasTransform.transform);

            characters[i] = characterObj;

            //The canvas itself is flipped 90 degrees, this compensates for it
            RectTransform rect = characterObj.GetComponent<RectTransform>();

            rect.position = new Vector3(0, 80, -400 + (100 * i + 1));

            TextMeshProUGUI characterNameText = characterObj.GetComponentInChildren<TextMeshProUGUI>();


            EntityScript arcadeOpponentEntity = arcadeStages[i].currentOpponent.GetComponent<EntityScript>();

            characterNameText.text = arcadeOpponentEntity.fighterName;
        }
    }

    private void Start()
    {
        arcadeRunData = GameObject.Find("ArcadeRunData").GetComponent<ArcadeRunData>();

        canvasTransform = GetComponent<RectTransform>();

        PreloadAllArcadeStages();
    }
}
