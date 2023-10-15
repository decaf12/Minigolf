using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public BallController ball;
    public TextMeshProUGUI labelPlayerName;
    public PlayerRecord playerRecord;
    private int playerIndex;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        playerIndex = 0;

        SetUpPlayer();
    }

    private void SetUpPlayer()
    {
        ball.SetUpBall(playerRecord.playerColours[playerIndex]);
        labelPlayerName.text = playerRecord.playerList[playerIndex].name;
    }

    public void NextPlayer(int prevPutts)
    {
        playerRecord.AddPutts(playerIndex, prevPutts);
        if (playerIndex < playerRecord.playerList.Count - 1)
        {
            ball.SetUpBall(playerRecord.playerColours[++playerIndex]);
        }
        else
        {
            if (playerRecord.levelIndex == playerRecord.levels.Length - 1)
            {
                // load the scoreboard scene
            }
            else
            {
                SceneManager.LoadScene(playerRecord.levels[++playerRecord.levelIndex]);
            }
        }
    }
}
