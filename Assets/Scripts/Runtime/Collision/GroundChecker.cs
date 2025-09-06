using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool drawGizmos;

    public Movement2D entityMove;

    public ContactFilter2D groundFilter;

    public BoxCollider2D boxCollider;

    [Header("Ground variables")]
    public bool grounded;

    public float groundYOffset;
    public float groundCheckRadius;

    [HideInInspector]
    public Vector3 snapPoint;

    public void GroundCheck()
    {
        RaycastHit2D[] results = new RaycastHit2D[10];

        int numHits = Physics2D.CircleCast(entityMove.transform.position + Vector3.down * groundYOffset, groundCheckRadius, Vector2.down, groundFilter, results, 0);

        for (int i = 0; i < numHits; i++)
        {
            if (results[i].collider != null)
            {
                //Check circlecast
                GroundConfirm(results[i]);
                return;
            }
            else
            {
                grounded = false;
            }
        }

        if (numHits < 1)
        {
            grounded = false;
        }
    }

    public void GroundConfirm(RaycastHit2D hitCheck)
    {
        RaycastHit2D[] hits = new RaycastHit2D[3];

        int num = Physics2D.CircleCastNonAlloc(entityMove.transform.position + Vector3.down * groundYOffset, groundCheckRadius, Vector2.down, hits, 0, groundFilter.layerMask);

        grounded = false;

        for (int i = 0; i < num; i++)
        {
            if (hits[i].collider.transform == hitCheck.transform)
            {
                snapPoint = Physics2D.ClosestPoint(hits[i].point + Vector2.up, hits[i].collider);

                entityMove.NewPosition(snapPoint);

                grounded = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(entityMove.transform.TransformPoint(Vector3.down * groundYOffset), groundCheckRadius);

            Gizmos.color = new Color(0.21f, 0.39f, 0.43f, 1);
            Gizmos.DrawCube(snapPoint, Vector3.one * 0.3f);

            Gizmos.color = new Color(0.3f, 0.2f, 0.8f);
            Gizmos.DrawCube(snapPoint, Vector3.one * 0.1f);
        }
    }
}
