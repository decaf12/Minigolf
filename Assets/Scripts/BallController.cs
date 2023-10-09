using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float maxPower;
    public float changeAngleSpeed;
    public float lineLength;
    private LineRenderer line;
    private Rigidbody ball;
    private float angle;
    private float powerUpTime;
    private float power;
    public float Power
    {
        get {return power;}
    }

    private float putts;

    public float Putts
    {
        get {return putts;}
    }

    void Awake()
    {
        ball = GetComponent<Rigidbody>();

        /* Cap on how fast the ball can spin.
           The default is too low. Raise it to 1000. */
        ball.maxAngularVelocity = 1000; 
        line = GetComponent<LineRenderer>();
        powerUpTime = 0;
        putts = 0;
        power = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ChangeAngle(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ChangeAngle(1);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Putt();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            PowerUp();
        }
        UpdateLinePositions();
    }

    private void ChangeAngle(int direction)
    {
       angle += changeAngleSpeed * Time.deltaTime * direction;
    }

    private void UpdateLinePositions()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    private void Putt()
    {
        ball.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);
        power = 0;
        powerUpTime = 0;
        ++putts;
    }

    private void PowerUp()
    {
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
    }
}