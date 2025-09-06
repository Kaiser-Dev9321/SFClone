using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeRunData : MonoBehaviour
{
    public int currentArcadeStageIndex = 0; //The first one

    public EntityScript arcadeCharacter;
    public EntityScript currentOpponentEntity;

    public ArcadeModeData fighterArcadeModeData;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
