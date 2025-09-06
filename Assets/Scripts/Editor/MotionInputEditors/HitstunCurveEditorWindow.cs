using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HitstunCurveEditorWindow : EditorWindow
{
    private string soName;
    private AnimationCurve animationCurveVelocityX;
    private AnimationCurve animationCurveVelocityY;

    private bool disableGroundCheck;
    private string animationName;

    [MenuItem("Fighter Creation Tools/Create hitstun curves")]
    static void Init()
    {
        HitstunCurveEditorWindow window = (HitstunCurveEditorWindow)EditorWindow.GetWindow(typeof(HitstunCurveEditorWindow));
        window.Show();
    }

    //Create stage elemnts if they are not in the scene
    private void CreateHitstunCurves()
    {
        HitstunData asset = ScriptableObject.CreateInstance<HitstunData>();

        asset.hitstunAnimationCurveX = animationCurveVelocityX;
        asset.hitstunAnimationCurveY = animationCurveVelocityY;

        asset.disableGroundCheck = disableGroundCheck;
        asset.animationName = animationName;

        EditorAssetSupport.assetName = soName;
        EditorAssetSupport.CheckAssetCreation(asset, $"Assets/Resources/TestHitstunCurveData");
    }

    private void OnGUI()
    {
        GUI.contentColor = new Color(0.4f, 0.7f, 0.3f);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Hitstun Curves Data Name:");

        soName = EditorGUILayout.TextField(soName);

        GUILayout.EndHorizontal();

        GUI.contentColor = new Color(0.8f, 0.05f, 0.01f);

        animationCurveVelocityX = EditorGUILayout.CurveField("Velocity X:", animationCurveVelocityX, Color.red, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(0.02f, 0.85f, 0.01f);

        animationCurveVelocityY = EditorGUILayout.CurveField("Velocity Y:", animationCurveVelocityY, Color.green, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(1, 1, 1, 1);

        disableGroundCheck = EditorGUILayout.Toggle("Disable Ground Check:", disableGroundCheck);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Hitstun Animation Name:");

        animationName = EditorGUILayout.TextField(animationName);

        GUILayout.EndHorizontal();

        GUILayout.Space(100);

        GUI.color = new Color(0.5f, 0.8f, 0.3f, 1);

        bool clearCurves = GUILayout.Button("Clear all curves", GUILayout.Height(25));

        if (clearCurves)
        {
            animationCurveVelocityX = EditorGUILayout.CurveField("Velocity X:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
            animationCurveVelocityY = EditorGUILayout.CurveField("Velocity Y:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
        }

        GUI.color = new Color(0, 0.8f, 0.03f, 1);

        bool motionCurveCreation = GUILayout.Button("Create hitstun curve data", GUILayout.Height(40));

        if (motionCurveCreation)
        {
            Debug.Log("Creating hitstun curves...");

            CreateHitstunCurves();
        }

        GUI.color = new Color(1, 1, 1, 1);
    }
}