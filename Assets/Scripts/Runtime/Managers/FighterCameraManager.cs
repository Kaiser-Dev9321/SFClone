using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FighterCameraManager : MonoBehaviour
{
    private FighterGameManager fighterGameManager;

    public CameraNibHandler cameraNibHandler;

    public CinemachineVirtualCamera startOfRoundCamera;
    public CinemachineVirtualCamera fightersTargetSuperCam;

    private CompositeCollider2D cameraLockTo;

    private void Awake()
    {
        fighterGameManager = Object.FindObjectOfType<FighterGameManager>();
    }

    private void LoadCamera(AsyncOperation stageScene)
    {
        if (stageScene.isDone)
        {
            cameraLockTo = GameObject.Find("CameraConfiner").GetComponent<CompositeCollider2D>();
        }
    }

    private void LockCamera()
    {
        CinemachineConfiner2D cameraLocker = fightersTargetSuperCam.GetComponent<CinemachineConfiner2D>();

        if (!cameraLockTo)
        {
            Debug.LogError("There is no CameraConfiner2D in this scene");
        }

        cameraLocker.m_BoundingShape2D = cameraLockTo;

        fightersTargetSuperCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraLockTo;
    }

    public void ChangeCamera(CinemachineVirtualCamera cVCam, CinemachineVirtualCamera lastCVCam)
    {
        cVCam.gameObject.SetActive(true);
        lastCVCam.gameObject.SetActive(false);
    }
}
