using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LeaderPlaneController : EnemyPlane
{
    public static DelegateHelper.GetController getController;
    public static DelegateHelper.SetController setController;
    
    public LeaderPlaneController():base()
    {
        MaximumSpeed = 4.5f;
        MaximumHealth = 2;
    }
}