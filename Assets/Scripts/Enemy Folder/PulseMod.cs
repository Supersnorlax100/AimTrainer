using UnityEngine;
using System.Collections;

public class PulseMod : MonoBehaviour
{
    float pulseMax = 2;
    float pulseMin = .4f;
    int scaleDirection = 1;

    void Start()
    {
        StartCoroutine(pulse());
    }

    IEnumerator pulse()
    {
        while (true)
        {
            if (transform.localScale.x >= pulseMax || transform.localScale.x <= pulseMin)
            {
                scaleDirection *= -1;
                transform.localScale = new Vector3(transform.localScale.x + (.1f * scaleDirection), transform.localScale.y + (.1f * scaleDirection), transform.localScale.z + (.1f * scaleDirection));
            }
            transform.localScale = new Vector3(transform.localScale.x + (.1f * scaleDirection), transform.localScale.y + (.1f * scaleDirection), transform.localScale.z + (.1f* scaleDirection) );
            yield return new WaitForSeconds(.2f);
        }
    }
}
