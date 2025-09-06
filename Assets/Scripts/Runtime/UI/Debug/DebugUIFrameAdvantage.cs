    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIFrameAdvantage : MonoBehaviour
{
    public TextMeshProUGUI frameAdvantageText, frameDisadvantageText;
    public bool debugMilliseconds = false;
    public bool shouldCalculateFramesAdvantage, shouldCalculateFramesDisadvantage = false;
    public bool wasHit = false;

    public bool isFighter1;

    private FighterGameplayManager gameplayManager;

    private float frameAdvantageTime, frameDisadvantageTime;

    private float ConvertMSToFrames(float frameTime)
    {
        return Mathf.Round(frameTime * 60);
    }

    private void SetFrameAdvantageText()
    {

    }

    private void DebugFrameAdvantage(EntityScript displayFighter, EntityScript otherFighter)
    {
        //TODO: Change to trigger on event
        if (!displayFighter.fighterInput.canAttack)
        {
            if (displayFighter.fighterComboManager.attacked_Entity && !wasHit)
            {
                wasHit = true;
            }
        }

        if (displayFighter.fighterInput.canAttack && wasHit)
        {
            shouldCalculateFramesAdvantage = true;

            frameAdvantageTime += Time.deltaTime;
        }
        else if (!otherFighter.entityHitstun.hitStunned && wasHit)
        {
            shouldCalculateFramesAdvantage = true;

            frameAdvantageTime -= Time.deltaTime;
        }

        if (displayFighter.fighterInput.canAttack && !otherFighter.entityHitstun.hitStunned && shouldCalculateFramesAdvantage)
        {
            shouldCalculateFramesAdvantage = false;
            wasHit = false;

            float frameAdvantage = ConvertMSToFrames(frameAdvantageTime);

            if (debugMilliseconds)
            {
                frameAdvantageText.text = $"{frameAdvantage}\n{frameAdvantageTime}";
            }
            else
            {
                frameAdvantageText.text = $"{frameAdvantage}";
            }

            if (frameAdvantage > 0)
            {
                frameAdvantageText.color = Color.green;
            }
            else if (frameAdvantage < 0)
            {
                frameAdvantageText.color = Color.red;
            }
            else
            {
                frameAdvantageText.color = Color.yellow;
            }

            frameAdvantageTime = 0;
        }
    }

    private void DebugFrameDisadvantage(EntityScript displayFighter, EntityScript otherFighter)
    {
        if (otherFighter.fighterInput.canAttack && wasHit)
        {
            shouldCalculateFramesDisadvantage = true;

            frameDisadvantageTime -= Time.deltaTime;
        }
        else if (!displayFighter.entityHitstun.hitStunned && wasHit)
        {
            shouldCalculateFramesDisadvantage = true;

            frameDisadvantageTime += Time.deltaTime;
        }

        if (!displayFighter.entityHitstun.hitStunned && otherFighter.fighterInput.canAttack && shouldCalculateFramesDisadvantage)
        {
            shouldCalculateFramesDisadvantage = false;

            float frameDisadvantage = ConvertMSToFrames(frameDisadvantageTime);

            if (debugMilliseconds)
            {
                frameDisadvantageText.text = $"{frameDisadvantage}\n{frameDisadvantageTime}";
            }
            else
            {
                frameDisadvantageText.text = $"{frameDisadvantage}";
            }

            if (frameDisadvantage > 0)
            {
                frameDisadvantageText.color = Color.green;
            }
            else if (frameDisadvantage < 0)
            {
                frameDisadvantageText.color = Color.red;
            }
            else
            {
                frameDisadvantageText.color = Color.yellow;
            }

            frameDisadvantageTime = 0;
        }
    }

    private void Start()
    {
        gameplayManager = FindObjectOfType<FighterGameplayManager>();
    }

    private void Update()
    {
        if (isFighter1)
        {
            DebugFrameAdvantage(gameplayManager.freezeStopManager.fighter1, gameplayManager.freezeStopManager.fighter2);
            DebugFrameDisadvantage(gameplayManager.freezeStopManager.fighter2, gameplayManager.freezeStopManager.fighter1);

            transform.position = gameplayManager.freezeStopManager.fighter1.transform.position + new Vector3(0, 4.5f, 0);
        }
        else
        {
            DebugFrameAdvantage(gameplayManager.freezeStopManager.fighter2, gameplayManager.freezeStopManager.fighter1);
            //DebugFrameDisadvantage(gameplayManager.freezeStopManager.fighter1, gameplayManager.freezeStopManager.fighter2);

            transform.position = gameplayManager.freezeStopManager.fighter2.transform.position + new Vector3(0, 4.5f, 0);
        }
    }
}
