using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputCardinalDirection
{
    None,
    UpBack,
    Back,
    DownBack,
    Down,
    DownForward,
    Forward,
    UpForward,
    Up
}

[System.Serializable]
public class FighterCardinalDirectionInput
{
    public float rawXDirection;
    public float rawYDirection;

    public float cardinalXDirection;
    public float cardinalYDirection;

    public float input_timePressed;

    public InputCardinalDirection direction;

    public void ConvertRawToCardinalDirection()
    {
        if (rawXDirection > 0.5f)
        {
            cardinalXDirection = 1;
        }
        else if (rawXDirection < -0.5f)
        {
            cardinalXDirection = -1;
        }

        if (rawXDirection > -0.5f && rawXDirection < 0.5f)
        {
            cardinalXDirection = 0;
        }

        if (rawYDirection > 0.5f)
        {
            cardinalYDirection = 1;
        }
        else if (rawYDirection < -0.5f)
        {
            cardinalYDirection = -1;
        }

        if (rawYDirection > -0.5f && rawYDirection < 0.5f)
        {
            cardinalYDirection = 0;
        }

        //Convert to cardinal direction

        if (cardinalXDirection == 0 && cardinalYDirection == 0)
        {
            //None
            direction = InputCardinalDirection.None;
        }

        if (cardinalXDirection == -1 && cardinalYDirection == 1)
        {
            //UpBack
            direction = InputCardinalDirection.UpBack;
        }

        if (cardinalXDirection == -1 && cardinalYDirection == 0)
        {
            //Back
            direction = InputCardinalDirection.Back;
        }

        if (cardinalXDirection == -1 && cardinalYDirection == -1)
        {
            //DownBack
            direction = InputCardinalDirection.DownBack;
        }

        if (cardinalXDirection == 0 && cardinalYDirection == -1)
        {
            //Down
            direction = InputCardinalDirection.Down;
        }

        if (cardinalXDirection == 1 && cardinalYDirection == -1)
        {
            //DownForward
            direction = InputCardinalDirection.DownForward;
        }

        if (cardinalXDirection == 1 && cardinalYDirection == 0)
        {
            //Forward
            direction = InputCardinalDirection.Forward;
        }

        if (cardinalXDirection == 1 && cardinalYDirection == 1)
        {
            //UpForward
            direction = InputCardinalDirection.UpForward;
        }

        if (cardinalXDirection == 0 && cardinalYDirection == 1)
        {
            //Up
            direction = InputCardinalDirection.Up;
        }
    }
}
