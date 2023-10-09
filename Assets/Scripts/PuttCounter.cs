using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuttCounter : MonoBehaviour
{
    public BallController ball;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = ball.Putts.ToString();
    }

    void LateUpdate()
    {
        text.text = ball.Putts.ToString();
    }
}
