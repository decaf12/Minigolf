using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputPlayerName;
    public PlayerRecord playerRecord;
    public Button buttonStart;

    public void ButtonAddPlayer()
    {
        playerRecord.AddPlayer(inputPlayerName.text);
        buttonStart.interactable = true;
    }
}
