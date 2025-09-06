using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterDebug_PickFighter : MonoBehaviour
{
    public GameObject fightersParent;
    public TESTFighterGameManager gameManager;
    public TestFighterGameplayManager gameplayManager;
    public FighterShowComboUIManager comboUIManager;
    public FreezeStopManager freezeStopManager;

    [Space]
    public InputActionAsset iPA;

    public GameObject[] listOfAvailableFighters;

    public int currentAvailableFighter1, currentAvailableFighter2; //Current available fighter for that fighter

    public bool allPicked = false;

    private int currentlySelectingFighter; //Selecting Fighter 1 or 2

    public bool pickedFighter1, pickedFighter2 = false;

    [HideInInspector]
    public GameObject fighter1ToDebug, fighter2ToDebug; //Prefab

    public GameObject fighter1Object, fighter2Object;

    private void PickFighter(InputAction.CallbackContext context)
    {
        if (allPicked)
        {
            return;
        }

        float inputX = context.ReadValue<float>();

        if (inputX < 0)
        {
            if (currentlySelectingFighter == 0)
            {
                currentAvailableFighter1--;
            }
            else if (currentlySelectingFighter == 1)
            {
                currentAvailableFighter2--;
            }
        }
        else if (inputX > 0)
        {
            if (currentlySelectingFighter == 0)
            {
                currentAvailableFighter1++;
            }
            else if (currentlySelectingFighter == 1)
            {
                currentAvailableFighter2++;
            }
        }

        currentAvailableFighter1 = Mathf.Clamp(currentAvailableFighter1, 0, listOfAvailableFighters.Length - 1);
        currentAvailableFighter2 = Mathf.Clamp(currentAvailableFighter2, 0, listOfAvailableFighters.Length - 1);

        if (currentlySelectingFighter == 0)
        {
            fighter1ToDebug = listOfAvailableFighters[currentAvailableFighter1];
        }
        else if (currentlySelectingFighter == 1)
        {
            fighter2ToDebug = listOfAvailableFighters[currentAvailableFighter2];
        }
    }

    private void SwapFighter()
    {
        if (currentlySelectingFighter == 0)
        {
            currentlySelectingFighter = 1;
        }
        else if (currentlySelectingFighter == 1)
        {
            currentlySelectingFighter = 0;
        }
    }

    private void ConfirmFighter()
    {
        if (currentlySelectingFighter == 0)
        {
            if (!pickedFighter1)
            {
                pickedFighter1 = true;

                print("Fighter 1 picked");
            }
        }
        else if (currentlySelectingFighter == 1)
        {
            if (!pickedFighter2)
            {
                pickedFighter2 = true;

                print("Fighter 2 picked");
            }
        }
    }

    private void OnEnable()
    {
        iPA.Enable();
    }

    private void OnDisable()
    {
        iPA.Disable();

        iPA.FindActionMap("UI")["MovementX"].performed -= PickFighter;
    }

    private void Start()
    {
        iPA.FindActionMap("UI")["MovementX"].performed += PickFighter;

        fighter1ToDebug = listOfAvailableFighters[0];
        fighter2ToDebug = listOfAvailableFighters[1];
    }

    private void Update()
    {
        if (pickedFighter1 && pickedFighter2)
        {
            if (!allPicked) //Enable everything
            {
                allPicked = true;

                gameManager.enabled = true;
                gameplayManager.enabled = true;
                comboUIManager.enabled = true;
                freezeStopManager.enabled = true;


                fighter1Object = Instantiate(fighter1ToDebug, new Vector3(-3, 4, 0), Quaternion.identity, fightersParent.transform);
                fighter1Object.SetActive(false);

                gameManager.fighter1 = fighter1Object.GetComponent<EntityScript>();
                freezeStopManager.fighter1 = gameManager.fighter1;


                fighter2Object = Instantiate(fighter2ToDebug, new Vector3(3, 4, 0), Quaternion.identity, fightersParent.transform);
                fighter2Object.SetActive(false);

                gameManager.fighter2 = fighter2Object.GetComponent<EntityScript>();
                freezeStopManager.fighter2 = gameManager.fighter2;

                gameplayManager.AssignManagersOnEntityLoad();


                fighter1Object.SetActive(true);
                fighter2Object.SetActive(true);
            }
        }

        if (!allPicked)
        {
            if (iPA.FindActionMap("Gameplay")["LightKick"].WasPressedThisFrame())
            {
                SwapFighter();
            }

            if (iPA.FindActionMap("Gameplay")["LightPunch"].WasPressedThisFrame())
            {
                ConfirmFighter();
            }
        }
    }

    private void OnGUI()
    {
        if (allPicked)
        {
            return;
        }

        GUI.color = Color.white;

        if (fighter1ToDebug)
        {
            if (pickedFighter1)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.red;
            }

            GUI.TextArea(new Rect(0, 50, 150, 25), $"Fighter 1: {fighter1ToDebug.name}");
        }
        else
        {
            GUI.color = Color.red;
            GUI.TextArea(new Rect(0, 50, 150, 25), $"Fighter 1:");
        }

        if (fighter2ToDebug)
        {
            if (pickedFighter2)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.red;
            }

            GUI.TextArea(new Rect(0, 100, 150, 25), $"Fighter 2: {fighter2ToDebug.name}");
        }
        else
        {
            GUI.color = Color.red;
            GUI.TextArea(new Rect(0, 100, 150, 25), $"Fighter 2:");
        }

        GUI.color = Color.white;

        GUI.TextArea(new Rect(120, 75, 195, 25), $"Fighter: {currentlySelectingFighter + 1}");

        if (!pickedFighter1 || !pickedFighter2)
        {
            GUI.color = Color.red;

            GUI.TextArea(new Rect(50, 150, 400, 25), $"Fighters not picked yet");

        }
        
        if (pickedFighter1 && pickedFighter2)
        {
            GUI.color = Color.green;

            GUI.TextArea(new Rect(50, 150, 200, 25), $"Fighters picked, debugging mode on");
        }

        GUI.color = Color.white;

        GUI.TextArea(new Rect(5, 200, 600, 25), "Select fighters with arrows, light kick to swap to fighter 1 or fighter 2, light punch for confirmation");
    }
}
