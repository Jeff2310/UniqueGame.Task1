using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlaneController : EnemyPlane
{
    NormalPlaneController() : base()
    {
        MaximumSpeed = 3.0f;
        MaximumHealth = 3;
    }
}
