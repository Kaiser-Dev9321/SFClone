using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArcadeModeData))]
public class ArcadeModeEditor : Editor
{
    ArcadeModeData arcadeInspector;

    private bool syncStageToFighterName;

    private float currentSyncButtonPressedTime;

    private bool syncButtonActive;

    private SerializedProperty stageProperty;

    public void OnEnable()
    {
        arcadeInspector = target as ArcadeModeData;
    }

    private void SyncStageToFighter()
    {
        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Ryu")
        {
            stageProperty.stringValue = "Stage_Ryu_Waterfall";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Ken")
        {
            stageProperty.stringValue = "Stage_Ken_PathFromTheDojo";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Mike")
        {
            stageProperty.stringValue = "Stage_Mike_MountRushmore";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Joe")
        {
            stageProperty.stringValue = "Stage_Joe_Trainyard";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Cody")
        {
            stageProperty.stringValue = "Stage_Cody_HugeBlock";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Lee")
        {
            stageProperty.stringValue = "Stage_Lee_GreatWallOfChina";
        }

        if (arcadeInspector.currentOpponent.GetComponent<EntityScript>().fighterName == "Pernin")
        {
            //stageProperty.stringValue  = "I don't know yet";
        }
    }

    private void UpdatePressedButton()
    {
        
        if (syncButtonActive && Time.realtimeSinceStartup > currentSyncButtonPressedTime + 2)
        {
            Debug.Log("Active false");

            syncButtonActive = false;
        }

        Repaint();
    }

    public override void OnInspectorGUI()
    {
        Color normalSyncButtonBG = new Color(1, 0.5f, 0);
        Color pressedSyncButtonBG = new Color(0.1f, 0.85f, 0.5f);

        Color normalSyncButtonText = new Color(1, 1, 1, 1);
        Color pressedSyncButtonText = new Color(0.875f, 0.5f, 0.25f, 1);


        serializedObject.Update();

        stageProperty = serializedObject.FindProperty("stageName");

        SerializedProperty opponentProperty = serializedObject.FindProperty("currentOpponent");
        SerializedProperty hasNextArcadeLadderProperty = serializedObject.FindProperty("hasNextArcadeLadder");

        EditorGUILayout.PropertyField(opponentProperty, new GUIContent("Current opponent:"));

        hasNextArcadeLadderProperty.boolValue = EditorGUILayout.Toggle("Has next opponent:", hasNextArcadeLadderProperty.boolValue);

        stageProperty.stringValue = EditorGUILayout.TextField("Opponent's stage:", stageProperty.stringValue);

        GUI.backgroundColor = syncButtonActive ? pressedSyncButtonBG : normalSyncButtonBG;
        GUI.contentColor  = syncButtonActive ? pressedSyncButtonText : normalSyncButtonText;

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        syncStageToFighterName = GUILayout.Button("Sync stage to fighter name", GUILayout.Width(400), GUILayout.Height(60));

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        if (syncStageToFighterName)
        {
            currentSyncButtonPressedTime = Time.realtimeSinceStartup;

            syncButtonActive = true;

            SyncStageToFighter();
        }

        GUI.backgroundColor = new Color(1, 1, 1, 1);
        GUI.contentColor = new Color(1, 1, 1, 1);

        if (arcadeInspector.hasNextArcadeLadder)
        {
            SerializedProperty nextArcadeLadderProperty = serializedObject.FindProperty("nextArcadeLadder");

            EditorGUILayout.PropertyField(nextArcadeLadderProperty, new GUIContent("Next opponent:"));
        }

        UpdatePressedButton();

        serializedObject.ApplyModifiedProperties();
    }
}