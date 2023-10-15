using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputPlayerName;
    public PlayerRecord playerRecord;
    public Button buttonStart;
    public Button buttonAddPlayer;

    public void ButtonAddPlayer()
    {
        playerRecord.AddPlayer(inputPlayerName.text);
        buttonStart.interactable = true;
        inputPlayerName.text = "";

        if (playerRecord.playerList.Count == playerRecord.playerColours.Length)
        {
            buttonAddPlayer.interactable = false;
        }
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene(playerRecord.levels[0]);
    }
}
