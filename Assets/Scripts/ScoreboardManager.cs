using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI names;
    public TextMeshProUGUI putts;
    private PlayerRecord playerRecord;

    void Start()
    {
        Cursor.visible = true;
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        names.text = "";
        putts.text = "";
        
        foreach (var player in playerRecord.GetScoreboardList())
        {
            names.text += $"{player.name}\n";
            putts.text += $"{player.totalPutts}\n";
        }
    }
    void Update()
    {
        putts.fontSize = names.fontSize;
    }

    public void ButtonReturnMenu()
    {
        Debug.Log("replay button");
        Destroy(playerRecord.gameObject);
        SceneManager.LoadScene("Menu");
    }
}
