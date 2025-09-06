using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FighterStageEditorWindow : EditorWindow
{
    [MenuItem("Fighter Creation Tools/Create stage elements")]
    static void Init()
    {
        FighterStageEditorWindow window = (FighterStageEditorWindow)EditorWindow.GetWindow(typeof(FighterStageEditorWindow));
        window.Show();
    }

    //Create stage elemnts if they are not in the scene
    private void CreateStageElements()
    {
        if (!GameObject.Find("StageMusic"))
        {
            Debug.Log("Creating stage music");

            GameObject musicParentObj = Instantiate(new GameObject(), null);
            musicParentObj.name = "StageMusic";

            GameObject musicObj = Instantiate(new GameObject(), musicParentObj.transform);
            musicObj.name = "[NAME]";
        }

        if (!GameObject.Find("GameplayManager"))
        {
            Debug.Log("Creating gameplay manager");

            GameObject gameplayManagerPrefab = Resources.Load<GameObject>("Prefabs/StageObjects/GameplayManager");

            GameObject gameplayManagerObj = Instantiate(gameplayManagerPrefab, null);
            gameplayManagerObj.name = "GameplayManager";
        }

        if (!GameObject.Find("RoundManager"))
        {
            Debug.Log("Creating round manager");

            GameObject roundManagerPrefab = Resources.Load<GameObject>("Prefabs/StageObjects/RoundManager");

            GameObject roundManagerObj = Instantiate(roundManagerPrefab, null);
            roundManagerObj.name = "RoundManager";
        }

        Debug.Log($"<color=#29f0f9>Created all stage elements</color>");
    }

    private void OnGUI()
    {
        bool stageCreation = GUILayout.Button("Create stage elements");

        if (stageCreation)
        {
            Debug.Log("Creating stage elements...");

            CreateStageElements();
        }
    }
}