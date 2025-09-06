using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MotionInputEditorWindow : EditorWindow
{
    private enum MovePerformState
    {
        Ground,
        Air
    }

    private string soName;

    private InputCardinalDirection[] cardinalInputs;

    private int previousAmountOfMotionInputs;
    private int amountOfMotionInputs;

    private Vector2 scrollPos;

    private bool[] motionInputsArray;
    private float[] timeLeniences;

    private FighterAttackButtons attackButtonEnum;
    private float buttonTimeLeniency;
    private string motionCommandID;
    private MovePerformState performMoveWhile;
    private string testMessageToPrint;
    private AttackData attackData;
    private VoicelineData voicelineData;
    private CommandType motionCommandType;

    private bool updateWindow = false;
    private static Vector2 buttonSize;

    [MenuItem("Fighter Creation Tools/Create motion input data")]
    static void Init()
    {
        buttonSize = new Vector2(70, 70);

        MotionInputEditorWindow window = (MotionInputEditorWindow)EditorWindow.GetWindow(typeof(MotionInputEditorWindow));
        window.Show();
    }

    private void UpdateTables()
    {
        motionInputsArray = new bool[amountOfMotionInputs];
        cardinalInputs = new InputCardinalDirection[amountOfMotionInputs];
        timeLeniences = new float[amountOfMotionInputs];

        previousAmountOfMotionInputs = amountOfMotionInputs;
    }


    //Create stage elemnts if they are not in the scene
    private void CreateSepcialMove()
    {
        FighterBaseMotionInputButtonData buttonAsset = ScriptableObject.CreateInstance<FighterBaseMotionInputButtonData>();
        buttonAsset.motionInputValues.buttonToPress = attackButtonEnum;
        buttonAsset.motionInputValues.previousMotionInputData = new FighterMotionInputData[amountOfMotionInputs];

        for (int i = 0; i < amountOfMotionInputs; i++)
        {
            FighterMotionInputData inputAsset = ScriptableObject.CreateInstance<FighterMotionInputData>();
            inputAsset.directionToPress = cardinalInputs[i];
            inputAsset.motionInputIndex = i + 1;
            inputAsset.timeLeniency = timeLeniences[i];

            EditorAssetSupport.assetName = soName;
            EditorAssetSupport.CheckAssetCreation(inputAsset, $"Assets/Resources/TestMotionInputData");

            buttonAsset.motionInputValues.previousMotionInputData[i] = inputAsset;
        }

        buttonAsset.motionInputValues.motionCommandID = motionCommandID;
        buttonAsset.motionInputValues.mustPerformInAir = performMoveWhile == MovePerformState.Air;
        buttonAsset.motionInputValues.mustPerformOnGround = performMoveWhile == MovePerformState.Ground;
        buttonAsset.testMessageToPrint = testMessageToPrint;
        buttonAsset.motionInputValues.timeLeniency = buttonTimeLeniency;
        buttonAsset.motionInputValues.attackData = attackData;
        buttonAsset.motionInputValues.voicelineData = voicelineData;
        buttonAsset.motionInputValues.commandType = motionCommandType;

        EditorAssetSupport.assetName = $"{soName}_Button";
        EditorAssetSupport.CheckAssetCreation(buttonAsset, $"Assets/Resources/TestMotionInputData");
    }

    private GUIContent GetArrowImage(InputCardinalDirection arrowFromCardinalInput)
    {
        Texture arrowImage;
        GUIContent returnArrowContent = new GUIContent();

        switch (arrowFromCardinalInput)
        {
            case InputCardinalDirection.None:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/InvalidSymbol.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.Up:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_Up.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.UpForward:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_UpForward.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.Forward:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_Forward.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.DownForward:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_DownForward.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.Down:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_Down.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.DownBack:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_DownBack.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.Back:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_Back.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
            case InputCardinalDirection.UpBack:
                arrowImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/Arrows/Arrow_UpBack.png", typeof(Texture));
                returnArrowContent = new GUIContent(arrowImage);

                break;
        }

        return returnArrowContent;
    }

    private GUIContent GetButtonImage()
    {
        Texture buttonImage;
        GUIContent returnButtonContent = new GUIContent();

        switch (attackButtonEnum)
        {
            case FighterAttackButtons.LightPunch:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/ButtonSymbols/PunchButton_Light.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            case FighterAttackButtons.LightKick:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/InvalidSymbol.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            case FighterAttackButtons.HeavyPunch:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/ButtonSymbols/PunchButton_Heavy.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            case FighterAttackButtons.HeavyKick:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/InvalidSymbol.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            case FighterAttackButtons.FiercePunch:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/ButtonSymbols/PunchButton_Fierce.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            case FighterAttackButtons.FierceKick:
                buttonImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Sprites/Editor/InvalidSymbol.png", typeof(Texture));
                returnButtonContent = new GUIContent(buttonImage);

                break;
            default:
                break;
        }

        return returnButtonContent;
    }

    private void DrawInputButton(int index)
    {
        EditorGUILayout.BeginHorizontal();

        if (cardinalInputs.Length == 0)
        {
            return;
        }

        if (index > cardinalInputs.Length - 1)
        {
            return;
        }

        GUIContent arrowContent = GetArrowImage(cardinalInputs[index]);

        motionInputsArray[index] = GUILayout.Button(arrowContent, GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y));

        if (motionInputsArray[index])
        {
            cardinalInputs[index] = InputCardinalDirection.None;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawAttackButton()
    {
        EditorGUILayout.Space(40);

        GUIContent buttonContent = GetButtonImage();

        GUILayout.Button(buttonContent, GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y));

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Button input:");

        attackButtonEnum = (FighterAttackButtons)EditorGUILayout.EnumPopup(attackButtonEnum);
        
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
        GUI.backgroundColor = new Color(0.08f, 0.9f, 0.01f);
        GUI.color = Color.green;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Motion command name:");

        soName = EditorGUILayout.TextField(soName);

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;
        GUI.color = Color.white;

        GUILayout.Space(30);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Amount of motion inputs:");

        amountOfMotionInputs = EditorGUILayout.IntSlider(amountOfMotionInputs, 0, 30);

        EditorGUILayout.EndHorizontal();

        //Update to match new set variable
        if (previousAmountOfMotionInputs != amountOfMotionInputs)
        {
            UpdateTables();
        }

        EditorGUILayout.LabelField("Motion Input Layout:");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.BeginHorizontal();

        if (amountOfMotionInputs > 0)
        {
            for (int i = 0; i < amountOfMotionInputs; i++)
            {
                DrawInputButton(i);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        //Debug.Log($"Cardinal input: {cardinalInputs.Length}\nAmount of inputs: {amountOfMotionInputs}");

        if (amountOfMotionInputs > 0)
        {
            for (int i = 0; i < amountOfMotionInputs; i++)
            {
                if (i < amountOfMotionInputs)
                {
                    cardinalInputs[i] = (InputCardinalDirection)EditorGUILayout.EnumPopup(cardinalInputs[i]);
                }
            }
        }



        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.blue;

        if (amountOfMotionInputs > 0)
        {
            for (int i = 0; i < amountOfMotionInputs; i++)
            {
                if (i < amountOfMotionInputs)
                {
                    timeLeniences[i] = EditorGUILayout.Slider(timeLeniences[i], 0, 2, GUILayout.Height(40));
                }
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        GUI.backgroundColor = Color.white;

        GUI.contentColor = Color.white;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Buttom Time Leniency:");

        buttonTimeLeniency = EditorGUILayout.Slider(buttonTimeLeniency, 0, 2);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Motion Command ID:");
        
        motionCommandID = EditorGUILayout.TextField(motionCommandID);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Perform on Ground or Air:");

        performMoveWhile = (MovePerformState) EditorGUILayout.EnumPopup(performMoveWhile);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Perform a Motion Input or Command Normal:");

        motionCommandType = (CommandType)EditorGUILayout.EnumPopup(motionCommandType);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Test message to print:");

        testMessageToPrint = EditorGUILayout.TextField(testMessageToPrint);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Attack data:");

        attackData = (AttackData) EditorGUILayout.ObjectField(attackData, typeof(AttackData), false);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Voiceline data:");

        voicelineData = (VoicelineData) EditorGUILayout.ObjectField(voicelineData, typeof(VoicelineData), false);

        EditorGUILayout.EndHorizontal();

        DrawAttackButton();

        GUILayout.Space(100);

        GUI.color = new Color(0.95f, 0.21f, 0.01f, 1);

        bool clearInputs = GUILayout.Button("Clear all inputs", GUILayout.Height(25));

        if (clearInputs)
        {
        }

        GUI.color = new Color(0.82f, 0.458f, 0.03f, 1);

        bool motionCurveCreation = GUILayout.Button("Create motion input data", GUILayout.Height(40));

        if (motionCurveCreation)
        {
            Debug.Log("Creating motion input data...");

            CreateSepcialMove();
        }

        GUI.color = new Color(1, 1, 1, 1);
    }
}