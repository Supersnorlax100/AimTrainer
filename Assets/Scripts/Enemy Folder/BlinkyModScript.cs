using System.Collections.Generic;
using UnityEngine;

public class BlinkyModScript : MonoBehaviour
{
    public List<Vector3> blinkPositions;
    int curentIndex = 0;
    public void GetHit()
    {
        transform.position = blinkPositions[curentIndex];
        curentIndex++;
    }
}
