using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeCorner : MonoBehaviour
{

    [SerializeField] float threshold = 2f;
    [SerializeField] Transform body;
    Transform followTarget;

    NavMeshAgent navMeshAgent;
    FollowTarget followBehaviour;

    Vector3 startPos;
    Vector3 endPos;

    Quaternion startRot;
    Quaternion endRot;

    float startTime = 0f;

    bool transitioning;


    float speed;
    [SerializeField] float lerpTime = 1f;



    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        followBehaviour = GetComponent<FollowTarget>();
        speed = followBehaviour.speed;
        followTarget = followBehaviour.target;
    }

    // Update is called once per frame
    void Update()
    {

        // Straight forward raycast
        if (transitioning)
        {
            //float lerpTime = threshold / speed;
            print("should be transitioning");
            transform.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / lerpTime);
            //transform.Rotate(-90f * ((Time.deltaTime) / lerpTime), 0f, 0f, Space.Self);
            transform.rotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / lerpTime);
            if ((Time.time - startTime) >= lerpTime)
            {
                transitioning = false;
                navMeshAgent.enabled = true;
            }
        }
        else
        {
            RayCastForward();
        }

    }

    private void RayCastForward()
    {
        RaycastHit[] hits = Physics.RaycastAll(body.position, body.forward, 100f);
        Debug.DrawRay(body.position, (body.forward) * 1000f, Color.blue, Time.deltaTime);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Environment")
            {
                print("raycast hit wall");
                //print(hit.distance);
                if (hit.distance < threshold)
                {
                    print("hit wall less than threshold");
                    TakeNinetyUp(hit);
                }
            }
        }


    }

    private void TakeNinetyUp(RaycastHit hit)
    {

        Debug.DrawRay(hit.point, hit.normal * 100f, Color.red, 5f);
        navMeshAgent.enabled = false;


        startPos = transform.position;
        endPos = hit.point + transform.up;

        startRot = transform.rotation;
        endRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        //Euler(Vector3.Cross(-transform.up, transform.right)); //Vector3.ProjectOnPlane((hit.point - startPos).normalized, hit.normal));


        transitioning = true;
        startTime = Time.time;
    }
}
