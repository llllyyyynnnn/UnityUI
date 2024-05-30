// Copyright © 2024 llllyyyynnnn. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LynnUI_Animations : ScriptableObject
{
    public static float Smooth(float current, float goal, float speed) => current + (Time.deltaTime * (goal - current) * speed);
    public static Color Smooth(Color current, Color goal, float speed) => new Color(Smooth(current.r, goal.r, speed), Smooth(current.g, goal.g, speed), Smooth(current.b, goal.b, speed), Smooth(current.a, goal.a, speed));
}
