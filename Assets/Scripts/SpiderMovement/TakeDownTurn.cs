using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeDownTurn : MonoBehaviour
{

    [SerializeField] Transform raycaster;
    [SerializeField] Transform groundCheck;
    [SerializeField] float lerpTime = 1f;

    [SerializeField] float yOffset = 1f;
    [SerializeField] float threshold = 2f;
    [SerializeField] float minThreshold = 1f;

    NavMeshAgent navMeshAgent;

    Vector3 startPos;
    Vector3 endPos;

    Quaternion startRot;
    Quaternion endRot;

    bool transitioning;

    float startTime;

    TakeCorner takeCorner;
    bool otherTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        takeCorner = GetComponent<TakeCorner>();

    }

    // Update is called once per frame
    void Update()
    {

        otherTransitioning = takeCorner.GetTransitioningState();

        if (transitioning)
        {
            //float lerpTime = threshold / speed;
            print("should be transitioning");
            float moveFraction = ((Time.time - startTime) / lerpTime);
            Vector3 temp = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / lerpTime);
            Vector3 offset = transform.up * ((Mathf.Sin(moveFraction * Mathf.PI) * yOffset));
            //new Vector3(0f, Mathf.Sin(moveFraction * Mathf.PI) * yOffset, 0f);
            transform.position = temp;
            transform.localPosition = transform.localPosition + offset;


            //transform.Rotate(-90f * ((Time.deltaTime) / lerpTime), 0f, 0f, Space.Self);
            transform.rotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / lerpTime);
            if ((Time.time - startTime) >= lerpTime)
            {
                transitioning = false;
                navMeshAgent.enabled = true;
            }
        }
        else if (otherTransitioning)
        {
            return;
        }
        else
        {
            CheckGround();
        }

    }

    private void CheckGround()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(groundCheck.position, groundCheck.forward, out hit);
        Debug.DrawRay(groundCheck.position, groundCheck.forward * 1000f, Color.blue, Time.deltaTime);

        if (!hasHit || hit.distance > 5f)
        {
            RayCastDownwards();
        }


    }

    private void RayCastDownwards()
    {
        RaycastHit[] hits = Physics.RaycastAll(raycaster.position, raycaster.forward, 100f);
        Debug.DrawRay(raycaster.position, (raycaster.forward) * 1000f, Color.blue, Time.deltaTime);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Environment")
            {
                print("raycast hit wall");
                //print(hit.distance);
                if (hit.distance < threshold && hit.distance > minThreshold)
                {
                    print("hit wall less than threshold");
                    TakeNinetyDown(hit);
                }
            }
        }


    }

    private void TakeNinetyDown(RaycastHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal * 100f, Color.red, 5f);
        navMeshAgent.enabled = false;


        startPos = transform.position;
        endPos = hit.point - transform.up;

        startRot = transform.rotation;
        endRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        //Euler(Vector3.Cross(-transform.up, transform.right)); //Vector3.ProjectOnPlane((hit.point - startPos).normalized, hit.normal));


        transitioning = true;
        startTime = Time.time;
    }

    public bool GetTransitioningState()
    {
        return transitioning;
    }
}
