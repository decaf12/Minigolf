using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    private Vector3 lastPosition;

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

        Vector3? worldPoint = CastMouseClickRay();
        if (!worldPoint.HasValue)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            Putt(worldPoint.Value);
        }
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            PowerUp();
        }
        UpdateLinePositions(worldPoint.Value);
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        return Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out RaycastHit hit, float.PositiveInfinity)
            ? hit.point
            : null;
    }

    private void UpdateLinePositions(Vector3 worldPoint)
    {
        if (holeTime == 0)
        {
            line.enabled = true;
        }
        worldPoint.y = transform.position.y;

        Vector3 deltaPosRaw = worldPoint - transform.position;
        Vector3 deltaPos = (deltaPosRaw.magnitude > 1 
            ? deltaPosRaw.normalized
            : deltaPosRaw)
            * lineLength;
        Vector3[] positions = {
            transform.position,
            transform.position + deltaPos
        };
        
        line.SetPositions(positions);
    }

    private void Putt(Vector3 worldPoint)
    {
        lastPosition = transform.position;
        ball.AddForce((worldPoint - lastPosition).normalized * maxPower * power, ForceMode.Impulse);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Out Of Bounds")
        {
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
            transform.position = lastPosition;
        }
    }
}