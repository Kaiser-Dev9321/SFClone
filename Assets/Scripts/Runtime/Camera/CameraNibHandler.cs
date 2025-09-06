using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraNibHandler : MonoBehaviour
{
    public GameManager gameManager;
    public Camera gameCamera;

    private Bounds bounds;
    private float cam_minX, cam_maxX;

    private float height;

    public GameObject displacement_Left, displacement_Right;
    private float displaced_Left_Value, displaced_Right_Value;

    private float displacedLerpValue;

    [SerializeField]
    private float displacedLerpAmount = 0.5f;

    private float centerBetweenFighters;

    [Space]
    [SerializeField]
    private float distanceBetweenFighters;

    [Space]
    [SerializeField]
    private float centerBetweenFightersAndDisplacedLerp;

    private float far_xUpdate;

    public float maximumDistanceBetweenFighters;

    [Space]
    [SerializeField]
    private float camMode = 0;

    /// <summary>
    /// TODO: A few bugs for this camera system:
    /// 
    /// It sometimes clips through when far and goes to a random position
    /// It gets jumpy when switching to close and far range camera mode
    /// 
    /// 
    /// 
    /// </summary>

    private void Start()
    {
        displacement_Left = GameObject.Find("DisplacedLeft");
        displacement_Right = GameObject.Find("DisplacedRight");

        height = gameCamera.orthographicSize * 2;

        displaced_Left_Value = displacement_Left.transform.position.x;
        displaced_Right_Value = displacement_Right.transform.position.x;
    }

    private void CloseCameraUpdate()
    {
        if (centerBetweenFighters < cam_minX)
        {
            float dist = Mathf.Abs(distanceBetweenFighters - displacedLerpValue);
            float crossDist = (15 - dist) * 0.01f;

            displacedLerpAmount -= crossDist * Time.deltaTime;

            //print($"<color=#9f058a>Camera move left close:{dist}\n</color>{crossDist}");
        }

        if (centerBetweenFighters > cam_maxX)
        {
            float dist = Mathf.Abs(distanceBetweenFighters - displacedLerpValue);
            float crossDist = (15 - dist) * 0.01f;

            displacedLerpAmount += crossDist * Time.deltaTime;

            //print($"<color=#ad4a84>Camera move right close: {dist}\n</color>{crossDist}");
        }

        transform.position = new Vector3(displacedLerpValue, 0);
    }

    // TODO: So for far camera I'm displacing by an amount by going above a max threshold
    // But I think it would be better to have it be a direct value
    // Calculate direct value by making it a valid lerp value to fit in displacedLerpValue?
    // Get distance between center of fighters and displaced lerp value, moving can then be offsetted

    private void FarCameraUpdate()
    {
        centerBetweenFightersAndDisplacedLerp = Vector3.Distance(new Vector3(centerBetweenFighters, 0, 0), new Vector3(displacedLerpValue, 0, 0));

        if (centerBetweenFighters < far_xUpdate)
        {
            if (centerBetweenFightersAndDisplacedLerp > 0.1f)
            {
                //displacedLerpAmount -= centerBetweenFightersAndDisplacedLerp * Time.deltaTime;

                //(displacedLerpValue - displaced_Right_Value) / (displaced_Right_Value - displaced_Left_Value)

                displacedLerpValue = centerBetweenFighters;

                //TODO: Try directly setting instead of translating to remove jumpy camera?

                far_xUpdate = centerBetweenFighters;

                //print($"<color=#943ffa>Camera move left far: {centerBetweenFightersAndDisplacedLerp}</color>");
                print($"<color=#943ffa>Camera move left far: {displacedLerpAmount}</color>");
            }
        }
        else if (centerBetweenFighters > far_xUpdate)
        {
            if (centerBetweenFightersAndDisplacedLerp > 0.1f)
            {
                //displacedLerpAmount += centerBetweenFightersAndDisplacedLerp * Time.deltaTime;

                //(displacedLerpValue - displaced_Right_Value) / (displaced_Right_Value - displaced_Left_Value)

                displacedLerpValue = centerBetweenFighters;

                //TODO: Try directly setting instead of translating to remove jumpy camera?

                far_xUpdate = centerBetweenFighters;

                //print($"<color=#68dcb6>Camera move right far: {centerBetweenFightersAndDisplacedLerp}</color>");
                print($"<color=#68dcb6>Camera move right far: {displacedLerpAmount}</color>");
            }
        }

        transform.position = new Vector3(displacedLerpValue, 0);
    }


    private void Update()
    {
        bounds = new Bounds(gameCamera.transform.position, new Vector3(height * gameCamera.aspect, height, 0));

        cam_minX = displacedLerpValue - 1;
        cam_maxX = displacedLerpValue + 1;

        if (camMode == 0)
        {
            displacedLerpValue = Mathf.Lerp(displaced_Left_Value, displaced_Right_Value, displacedLerpAmount);
        }

        displacedLerpAmount = Mathf.Clamp(displacedLerpAmount, 0, 1);

        centerBetweenFighters = Mathf.Lerp(gameManager.fighter1.transform.position.x, gameManager.fighter2.transform.position.x, 0.5f);
        distanceBetweenFighters = Vector3.Distance(new Vector3(gameManager.fighter1.transform.position.x, 0, 0), new Vector3(gameManager.fighter2.transform.position.x, 0, 0));

        if (distanceBetweenFighters < maximumDistanceBetweenFighters)
        {
            camMode = 0;
            CloseCameraUpdate();
        }
        else
        {
            if (camMode != 1)
            {
                far_xUpdate = centerBetweenFighters;
            }

            camMode = 1;

            if (distanceBetweenFighters < 15)
            {
                FarCameraUpdate();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawLine(new Vector3(displaced_Left_Value, 4, 0), new Vector3(displaced_Left_Value, 8, 0), 6);
        Handles.DrawLine(new Vector3(displaced_Right_Value, 4, 0), new Vector3(displaced_Right_Value, 8, 0), 6);

        //Displaced lerp value, top
        Gizmos.color = new Color(0.9f, 0.5f, 0.04f, 0.75f);
        Gizmos.DrawSphere(new Vector3(displacedLerpValue, 6, 0), 0.4f);

        //Far x update, middle
        Gizmos.color = new Color(0.37f, 0.35f, 0.75f, 0.75f);
        Gizmos.DrawSphere(new Vector3(far_xUpdate, 5, 0), 0.4f);

        //Center between fighters, bottom
        Gizmos.color = new Color(0.07f, 0.25f, 0.95f, 0.75f);
        Gizmos.DrawSphere(new Vector3(centerBetweenFighters, 4, 0), 0.4f);

        //Close cam distances
        if (camMode == 0)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(new Vector3(cam_minX, 0, 0), new Vector3(cam_minX, 0, 0) + Vector3.up * 4, 4);
            Handles.DrawLine(new Vector3(cam_maxX, 0, 0), new Vector3(cam_maxX, 0, 0) + Vector3.up * 4, 4);
        }
    }
}
