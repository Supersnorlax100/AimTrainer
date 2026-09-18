using UnityEngine;

public class BlinkyScript : TargetScript
{

    Vector3[] blinkPositions;
    Vector3 pos;
    int curentIndex = 0;

    private void Start()
    {
        SetHealthBar();
        pos = blinkPositions[0];
        curentIndex = 0;
    }

    public override void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        base.GetHit(damage, isCrit, critMultiplier);
        curentIndex++;
        pos = blinkPositions[curentIndex];
    }
}
