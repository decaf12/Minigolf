using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSliderController : MonoBehaviour
{
    public BallController ball;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        slider.value = ball.Power / ball.maxPower;
    }
}
