using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformList : MonoBehaviour
{
    private Movement2D movement2D;
    public Transform[] moveTransforms;

    private void Start()
    {
        movement2D = GetComponent<Movement2D>();
    }

    public void UpdateMoveTransform(int moveTransformIndex)
    {
        movement2D.transformToOffset = moveTransforms[moveTransformIndex];
    }
}
