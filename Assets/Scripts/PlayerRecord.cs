using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
    public class Player
    {
        public string name;
        public Color colour;
        public int[] putts;

        public Player(string newName, Color newColour, int levelCount)
        {
            name = newName;
            colour = newColour;
            putts = new int[levelCount];
        }
    }

    public List<Player> playerList;
    public string[] levels;
    public Color[] playerColours;
    public void AddPlayer(string name)
    {
        playerList.Add(new Player(name, playerColours[playerList.Count], levels.Length));
    }

    void OnEnable()
    {
        playerList = new List<Player>();
    }
}
