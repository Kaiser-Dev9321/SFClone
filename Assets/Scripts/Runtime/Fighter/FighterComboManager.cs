using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterComboManager : MonoBehaviour
{
    [HideInInspector]
    public EntityScript entity;

    public AttackEffectData currentPerformedAttackEffect;

    //[HideInInspector]
    public AttackData currentPerformedAttack;

    [HideInInspector]
    public AttackData lastPerformedAttack;

    public bool currentAttackHit = false;
    public bool lastAttackHit = false;

    public EntityScript attacker_Entity;
    public EntityScript attacked_Entity;

    public int combo_activeCount;

    public int lastTC_Combo_Check;

    public bool currentlyInACombo = false;

    private void OnEnable()
    {
        entity = GetComponent<EntityScript>();
    }

    public void ResetCombo()
    {
        //print("<b>Reset combo, attacker is not in hitstun</b>");

        combo_activeCount = 0;

        //text.text = $"{combo_activeCount} hit\nCombo";

        entity.fighterAttackManager.ResetDamageScaling();

        entity.fighterJuggleStateManager.ResetJuggle();

        currentlyInACombo = false;
    }

    public void AddCombo(EntityScript attackedEntity)
    {
        attacked_Entity = attackedEntity;

        combo_activeCount++;

        currentlyInACombo = true;

        //text.text = $"{combo_activeCount} hit\nCombo";

        //print($"<color=#993300>Combo increased: {combo_activeCount} </color>");
    }

    public void AssignCurrentPerformedAttackEffectData(AttackEffectData attackEffectData)
    {
        currentPerformedAttackEffect = attackEffectData;
    }

    private void Update()
    {
        if (attacked_Entity)
        {
            if (!attacked_Entity.entityHitstun.hitStunned || attacked_Entity.onTheGround)
            {
                ResetCombo();
                attacked_Entity = null;
            }
        }

        //TODO: Temporary fix, AI glitch where it can't hit if juggle is more than 0
        if (!attacked_Entity && entity.fighterJuggleStateManager.currentJuggle_Amount > 0)
        {
            ResetCombo();
        }
    }
}
