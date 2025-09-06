using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class ObjectReplacerWindow : EditorWindow
{
    public GameObject replaceWithPrefab;

    GUIStyle bStyle;

    void OnEnable()
    {
        bStyle = new GUIStyle(EditorStyles.miniButton);
        bStyle.normal.textColor = new Color(1, 0.5f, 0);
        bStyle.onFocused.textColor = Color.white;
        bStyle.fontSize = 18;
    }

    [MenuItem("Window/ObjectReplacer")]
    public static void ShowWindow()
    {
        GetWindow<ObjectReplacerWindow>("Object replacer window");
    }

    void OnGUI()
    {
        GUILayout.Label("Object replacer Window", EditorStyles.boldLabel);

        replaceWithPrefab = (GameObject)EditorGUILayout.ObjectField("Replace with:", replaceWithPrefab, typeof(GameObject), true);

        bool replaceButton = GUILayout.Button("Replace objects", bStyle, GUILayout.Width(200), GUILayout.Height(80));

        if (replaceButton)
        {
            if (replaceWithPrefab)
            {
                for (int i = Selection.gameObjects.Length - 1; i >= 0; --i)
                {
                    Debug.Log("Replaced: " + Selection.gameObjects[i].name + " with: " + replaceWithPrefab.name);
                    GameObject o = (GameObject)PrefabUtility.InstantiatePrefab(replaceWithPrefab);
                    o.transform.position = Selection.gameObjects[i].transform.position;
                    o.transform.parent = Selection.gameObjects[i].transform.parent;

                    //Instantiate(replaceWithPrefab, Selection.gameObjects[i].transform.position, Quaternion.identity, Selection.gameObjects[i].transform.parent);
                    DestroyImmediate(Selection.gameObjects[i], false);
                }
            }
            else
            {
                Debug.LogError("Replaced prefab is null, set on GUI.");
            }
        }
    }
}