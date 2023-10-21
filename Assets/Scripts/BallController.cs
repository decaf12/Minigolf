using System;
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
    public float lineLengthMultiplier;
    private LineRenderer line;
    private TrailRenderer trail;
    private Rigidbody ball;
    public float minHoleTime;
    private float powerPercent;
    public float Power
    {
        get {return powerPercent;}
    }

    private int putts;
    private float holeTime;

    public int Putts
    {
        get { return putts; }
    }

    private Vector3 lastPosition;
    private Vector3 rawCueLine;
    public Transform startTransform;
    public LevelManager levelManager;

    void Awake()
    {
        ball = GetComponent<Rigidbody>();

        /* Cap on how fast the ball can spin.
           The default is too low. Raise it to 1000. */
        ball.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        trail = GetComponent<TrailRenderer>();
        putts = 0;
        powerPercent = 0;
        holeTime = 0;
        startTransform.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (ball.velocity.magnitude > 0.01f || holeTime > 0)
        {
            line.enabled = false;
            powerPercent = 0;
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();
        if (!worldPoint.HasValue)
        {
            return;
        }

        rawCueLine = CalculateRawCueLine(worldPoint.Value);

        if (Input.GetMouseButtonDown(0))
        {
            line.enabled = true;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!line.enabled)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                line.enabled = false;
                powerPercent = 0;
                rawCueLine = Vector3.zero;
            }
            else
            {
                UpdatePower();
            }
            UpdateLinePositions();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (line.enabled)
            {
                Putt(worldPoint.Value);
            }
            line.enabled = false;
        }
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

    private Vector3 CalculateRawCueLine(Vector3 worldPoint)
    {
        worldPoint.y = transform.position.y;

        return worldPoint - transform.position;
    }
    private void UpdatePower()
    {
        powerPercent = Math.Min(rawCueLine.magnitude, 1);
    }
    private void UpdateLinePositions()
    {
        Vector3 cueLine = (rawCueLine.magnitude > 1
            ? rawCueLine.normalized
            : rawCueLine)
            * lineLengthMultiplier;

        if (Physics.Raycast(transform.position, cueLine, out RaycastHit hit, cueLine.magnitude))
        {
            if (hit.collider?.tag == "Course")
            {
                cueLine = hit.point - transform.position;
            }
        }

        Vector3[] positions = {
            transform.position,
            transform.position + cueLine
        };

        line.SetPositions(positions);
    }

    private void Putt(Vector3 worldPoint)
    {
        lastPosition = transform.position;
        ball.AddForce((worldPoint - lastPosition).normalized * maxPower * powerPercent, ForceMode.Impulse);
        powerPercent = 0;
        ++putts;
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
            // Debug.Log($"I'm in the hole and it only took me {putts} putts to get it in.");
            levelManager.NextPlayer(putts);
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

    public void SetUpBall(Color colour)
    {
        transform.position = startTransform.position;
        trail.Clear();

        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().material.color = colour;
        line.material.SetColor("_Color", colour);
        putts = 0;
    }
}