using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : MonoBehaviour
{
    public Rigidbody Target;
    public Vector3 TargetCenter;
    public float Radius = 2.5f;
    public int Rows = 5;
    public int TargetsInRow = 10;
    public Rigidbody Projectile;
    public float Step = 0.2f;
    public float Vel = 10f;
    public float Ang = 0.2f;
    public float Strength = 10f;
    public float MinVel = 5f;
    public float MaxVel = 40f;
    public Transform Meeter;

    float downtime;
    float tention;
    float rotX = 0f;
    float rotY = 0f;
    Quaternion originalRot;
    List<Rigidbody> createdObjects;

    float Downtime
    {
        get
        {
            return downtime;
        }

        set
        {
            tention = MinVel + Mathf.Clamp(Strength * downtime, 0f, MaxVel-MinVel);
            float h = tention / 100f;
            Meeter.localPosition = new Vector3(-.7f, -.3f + h / 2, 1f);
            Meeter.localScale = new Vector3(.1f, h, .1f);
            downtime = value;
        }
    }

    private void Start()
    {
        originalRot = transform.localRotation;
        createdObjects = new List<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("a"))
            transform.position -= Step * transform.right;
        if (Input.GetKey("d"))
            transform.position += Step * transform.right;
        if (Input.GetKey("s"))
            transform.position -= Step * transform.forward;
        if (Input.GetKey("w"))
            transform.position += Step * transform.forward;
        if (Input.GetKeyDown("space"))
        {
            Downtime = 0f;
        }
        if (Input.GetKey("space"))
            Downtime += Time.deltaTime;
        if (Input.GetKeyUp("space"))
        {
            Vector3 vel = Quaternion.AngleAxis(-Ang, transform.right) * (tention * transform.forward);
            Rigidbody rb = Instantiate(Projectile,
                transform.position+transform.forward,
                transform.rotation);
            createdObjects.Add(rb);
            rb.velocity = vel;
            Downtime = 0f;
        }
        if (Input.GetKeyDown("return"))
        {
            for (int x = 0; x < Rows; x++)
            {
                for (int i = 0; i < TargetsInRow; i++)
                {
                    // calculate angle for target
                    float ang = (i + (x % 2) * 0.5f) * 360f / TargetsInRow;
                    // make a rotation for that angle (in the up-direction)
                    Quaternion rot = Quaternion.AngleAxis(ang, Vector3.up);
                    // calculate position from center:
                    // center + rotated forward direction + height
                    Vector3 pos = TargetCenter +
                        rot * (Radius * Vector3.forward) +
                        x * Vector3.up;
                    createdObjects.Add(Instantiate(Target, pos, rot));
                    Downtime = 0f;
                }
            }
        }

        if (Input.GetKeyDown("x"))
        {
            foreach (var go in createdObjects)
                Destroy(go.gameObject);
            createdObjects.Clear();
        }

        rotX += Input.GetAxis("Mouse X") * 10f;
        rotY += Input.GetAxis("Mouse Y") * 10f;
        transform.localRotation = Quaternion.Euler(rotY, -rotX, 0f) * originalRot;
    }
}
