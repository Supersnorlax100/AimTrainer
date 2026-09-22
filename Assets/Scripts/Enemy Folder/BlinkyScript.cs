using System.Collections.Generic;
using UnityEngine;

public class BlinkyScript : TargetScript
{

    public List<Vector3> blinkPositions;
    int curentIndex = 0;

    private void Start()
    {
        SetHealthBar();
        transform.position = blinkPositions[curentIndex];
        curentIndex = 0;
    }

    public override void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        base.GetHit(damage, isCrit, critMultiplier);
        curentIndex++;
        transform.position = blinkPositions[curentIndex];
    }
}
