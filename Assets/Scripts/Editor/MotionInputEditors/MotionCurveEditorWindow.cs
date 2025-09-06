using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MotionCurveEditorWindow : EditorWindow
{
    private string soName;
    private AnimationCurve animationCurveVelocityX;
    private AnimationCurve animationCurveVelocityY;

    private AnimationCurve animationCurveTranslateX;
    private AnimationCurve animationCurveTranslateY;

    [MenuItem("Fighter Creation Tools/Create motion curves")]
    static void Init()
    {
        MotionCurveEditorWindow window = (MotionCurveEditorWindow)EditorWindow.GetWindow(typeof(MotionCurveEditorWindow));

        window.Show();
    }

    private void ResetAnimationCurves()
    {
        animationCurveVelocityX = EditorGUILayout.CurveField("Velocity X:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
        animationCurveVelocityY = EditorGUILayout.CurveField("Velocity Y:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
        animationCurveTranslateX = EditorGUILayout.CurveField("Translate X:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
        animationCurveTranslateY = EditorGUILayout.CurveField("Translate Y:", new AnimationCurve(), Color.red, new Rect(), GUILayout.Height(40));
    }

    //Create stage elemnts if they are not in the scene
    private void CreateMotionCurves()
    {
        MotionCurveData asset = ScriptableObject.CreateInstance<MotionCurveData>();

        asset.animationCurve_velocityX = animationCurveVelocityX;
        asset.animationCurve_velocityY = animationCurveVelocityY;

        asset.animationCurve_translateX = animationCurveTranslateX;
        asset.animationCurve_translateY = animationCurveTranslateY;

        EditorAssetSupport.assetName = soName;
        EditorAssetSupport.CheckAssetCreation(asset, $"Assets/Resources/TestMotionCurveData");
    }

    private void CreateGUI()
    {
        ResetAnimationCurves();
    }

    private void OnGUI()
    {
        GUI.contentColor = new Color(0.4f, 0.7f, 0.3f);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Motion Curves Data Name:");

        soName = EditorGUILayout.TextField(soName);

        GUILayout.EndHorizontal();

        GUI.contentColor = new Color(0.8f, 0.05f, 0.01f);

        animationCurveVelocityX = EditorGUILayout.CurveField("Velocity X:", animationCurveVelocityX, Color.red, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(0.02f, 0.85f, 0.01f);

        animationCurveVelocityY = EditorGUILayout.CurveField("Velocity Y:", animationCurveVelocityY, Color.green, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(1, 1, 1, 1);

        GUILayout.Space(10);

        GUI.contentColor = new Color(0.8f, 0.05f, 0.01f);

        animationCurveTranslateX = EditorGUILayout.CurveField("Translate X:", animationCurveTranslateX, Color.red, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(0.02f, 0.85f, 0.01f);

        animationCurveTranslateY = EditorGUILayout.CurveField("Translate Y:", animationCurveTranslateY, Color.green, new Rect(), GUILayout.Height(40));

        GUI.contentColor = new Color(1, 1, 1, 1);

        GUILayout.Space(100);

        GUI.color = new Color(0.5f, 0.8f, 0.3f, 1);

        bool clearCurves = GUILayout.Button("Clear all curves", GUILayout.Height(25));

        if (clearCurves)
        {
            ResetAnimationCurves();
        }

        GUI.color = new Color(0, 0.8f, 0.03f, 1);

        bool motionCurveCreation = GUILayout.Button("Create motion curve data", GUILayout.Height(40));

        if (motionCurveCreation)
        {
            Debug.Log("Creating motion curves...");

            CreateMotionCurves();
        }

        GUI.color = new Color(1, 1, 1, 1);
    }
}