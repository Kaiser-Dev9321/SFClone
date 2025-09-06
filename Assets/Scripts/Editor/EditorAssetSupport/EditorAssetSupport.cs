using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorAssetSupport
{
    public static string assetName;
    public static float assetIndex;

    //Repeat until nil
    private static void CheckIfAssetExists(Object asset, string filePath)
    {
        assetIndex++;

        Debug.Log(assetIndex);

        Object assetExists = AssetDatabase.LoadAssetAtPath($"{filePath}/{assetName}_{assetIndex}.asset", asset.GetType());

        if (assetExists)
        {
            CheckIfAssetExists(asset, filePath);
        }
        else
        {
            CreateAsset(asset, filePath);
        }
    }

    public static void CheckAssetCreation(Object asset, string filePath)
    {
        assetIndex = 1;

        Object assetExists = AssetDatabase.LoadAssetAtPath($"{filePath}/{assetName}_{assetIndex}.asset", asset.GetType());

        if (assetExists)
        {
            CheckIfAssetExists(asset, filePath);
        }
        else
        {
            CreateAsset(asset, filePath);
        }
    }

    private static void CreateAsset(Object asset, string filePath)
    {
        Debug.Log($"Created asset at: {filePath}, {assetName}_{assetIndex}");
        AssetDatabase.CreateAsset(asset, $"{filePath}/{assetName}_{assetIndex}.asset");
        AssetDatabase.SaveAssets();
    }
}
