using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
    public class Player
    {
        public string name;
        public Color colour;
        public int[] putts;
        public int totalPutts;

        public Player(string newName, Color newColour, int levelCount)
        {
            name = newName;
            colour = newColour;
            putts = new int[levelCount];
            totalPutts = 0;
        }
    }

    public List<Player> playerList;
    public string[] levels;
    public Color[] playerColours;

    [HideInInspector]
    public int levelIndex;

    public List<Player> GetScoreboardList()
    {
        foreach (Player player in playerList)
        {
            foreach (int puttScore in player.putts)
            {
                player.totalPutts += puttScore;
            }
        }
        return (from p in playerList orderby p.totalPutts select p).ToList();
    }

    public void AddPlayer(string name)
    {
        playerList.Add(new Player(name, playerColours[playerList.Count], levels.Length));
    }

    public void AddPutts(int playerIndex, int puttCount)
    {
        playerList[playerIndex].putts[levelIndex] = puttCount;
    }

    void OnEnable()
    {
        playerList = new List<Player>();
        DontDestroyOnLoad(gameObject);
    }
}
