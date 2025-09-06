using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EntityScript fighter1;
    public EntityScript fighter2;

    public Transform fightersEmptyTransform;

    public virtual void PutFighterBackInFightersEmptyTransform(Transform fighterTransform)
    {
        fightersEmptyTransform.parent = fightersEmptyTransform;
    }

    private void Update()
    {
        if (fighter1.fighterInput.canAttack && !fighter1.entityHitstun.hitStunned)
        {
            if (fighter1.transform.position.x > fighter2.transform.position.x)
            {
                if (fighter1.entityMovement.groundChecker.grounded)
                {
                    fighter1.FlipXDirection(true);
                }

                fighter1.actualDirectionX = -1;
            }
            else if (fighter1.transform.position.x <= fighter2.transform.position.x)
            {
                if (fighter1.entityMovement.groundChecker.grounded)
                {
                    fighter1.FlipXDirection(false);
                }

                fighter1.actualDirectionX = 1;
            }
        }

        if (fighter2.fighterInput.canAttack && !fighter2.entityHitstun.hitStunned)
        {
            if (fighter2.transform.position.x > fighter1.transform.position.x)
            {
                if (fighter2.entityMovement.groundChecker.grounded)
                {
                    fighter2.FlipXDirection(true);
                }

                fighter2.actualDirectionX = -1;
            }
            else if (fighter2.transform.position.x <= fighter1.transform.position.x)
            {
                if (fighter2.entityMovement.groundChecker.grounded)
                {
                    fighter2.FlipXDirection(false);
                }

                fighter2.actualDirectionX = 1;
            }
        }
    }
}