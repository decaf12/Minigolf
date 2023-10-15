using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BallController ball;
    public PlayerRecord playerRecord;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
    }

    private void SetUpPlayer()
    {
        
    }
}
