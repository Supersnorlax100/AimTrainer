using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        EventHandeler.onTargetDeath += AddScore;
    }

    public void AddScore()
    {
        PlayerControler.instance.score += (PlayerControler.instance.target.GetComponent<TargetScript>().scoreValue) ;
    }
}
