using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_CharacterTable : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    public UI_CharacterCell[] characterCells;

    public UI_CharacterCell currentCharacterCell;
    public int characterIndex;

    [HideInInspector]
    public SelectArcadeCharacter selectArcadeCharacter;

    [HideInInspector]
    public ArcadeLadderLoader arcadeLadderLoader;

    public void AssignArcadeCharacter()
    {
        if (currentCharacterCell.fighterGameObject)
        {
            selectArcadeCharacter.NewStoredFighter(currentCharacterCell.fighterGameObject);
        }

        if (currentCharacterCell.fighterArcadeModeData)
        {
            arcadeLadderLoader.LoadNewArcadeModeData(currentCharacterCell.fighterArcadeModeData);
        }
    }

    public void MoveToNewCharacterIndex(int newCharacterIndex)
    {
        UI_CharacterCell previousCharacterCell = currentCharacterCell;

        characterIndex = newCharacterIndex;
        currentCharacterCell = characterCells[characterIndex];

        previousCharacterCell.OnExitFunction();
        currentCharacterCell.OnEnterFunction();
    }

    //TODO: Replace stuff

    private void OnUIMoveX(InputAction.CallbackContext context)
    {
        float movementX = context.ReadValue<float>();

        if (movementX > 0)
        {
            //print("Moved right");

            currentCharacterCell.MoveRightFunction();
        }
        else if (movementX < 0)
        {
            //print("Moved left");

            currentCharacterCell.MoveLeftFunction();
        }
    }

    private void OnUIMoveY(InputAction.CallbackContext context)
    {
        float movementY = context.ReadValue<float>();

        if (movementY > 0)
        {
            //print("Moved up");

            currentCharacterCell.MoveUpFunction();
        }
        else if (movementY < 0)
        {
            //print("Moved down");

            currentCharacterCell.MoveDownFunction();
        }
    }

    private void Start()
    {
        currentCharacterCell = characterCells[0];
        currentCharacterCell.OnEnterFunction();

        AssignArcadeCharacter();

        //print("Movement assigned");

        inputActionAsset.FindActionMap("UI")["MovementX"].performed += OnUIMoveX;
        inputActionAsset.FindActionMap("UI")["MovementY"].performed += OnUIMoveY;
    }

    private void OnEnable()
    {
        inputActionAsset.Enable();
    }

    private void OnDisable()
    {
        inputActionAsset.FindActionMap("UI")["MovementX"].performed -= OnUIMoveX;
        inputActionAsset.FindActionMap("UI")["MovementY"].performed -= OnUIMoveY;

        inputActionAsset.Disable();
    }

    private void Update()
    {
        if (inputActionAsset.FindActionMap("UI")["Press"].WasPressedThisFrame())
        {
            currentCharacterCell.OnPressFunction();
        }

        //print(currentCharacterCell.gameObject.name);
    }
}
