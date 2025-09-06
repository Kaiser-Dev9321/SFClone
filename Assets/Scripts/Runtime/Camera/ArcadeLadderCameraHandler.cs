using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeLadderCameraHandler : MonoBehaviour
{
    public Camera thisCamera;

    public void SetNewCameraPosition(Vector3 newPos)
    {
        thisCamera.transform.position = newPos;    }
}
