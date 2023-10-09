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
    public float minHoleTime;
    private float angle;
    private float powerUpTime;
    private float power;
    public float Power
    {
        get {return power;}
    }

    private float putts;
    private float holeTime;

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
        holeTime = 0;
    }

    void Update()
    {
        if (ball.velocity.magnitude > 0.01f)
        {
            line.enabled = false;
            return;
        }

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
        if (holeTime == 0)
        {
            line.enabled = true;
        }
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


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole")
        {
            CountHoleTime();
        }
    }

    private void CountHoleTime()
    {
        holeTime += Time.deltaTime;
        if (holeTime >= minHoleTime)
        {
            Debug.Log($"I'm in the hole and it only took me {putts} putts to get it in.");
            holeTime = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            LeftHole();
        }
    }

    private void LeftHole()
    {
        holeTime = 0;
    }
}