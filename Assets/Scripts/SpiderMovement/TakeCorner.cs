using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeCorner : MonoBehaviour
{

    [SerializeField] float threshold = 1f;
    [SerializeField] Transform body;

    NavMeshAgent navMeshAgent;
    FollowTarget followBehaviour;

    Vector3 startPos;
    Vector3 endPos;

    Quaternion startRot;
    Quaternion endRot;

    float startTime = 0f;

    bool transitioning;


    float speed;



    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        followBehaviour = GetComponent<FollowTarget>();
        speed = followBehaviour.speed;
    }

    // Update is called once per frame
    void Update()
    {

        // Straight forward raycast
        if (transitioning)
        {
            print("should be transitioning");
            transform.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / (threshold / speed));
            transform.rotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / (threshold / speed));
        }
        else
        {
            RayCastForward();
        }

    }

    private void RayCastForward()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(body.position, body.forward, out hit);
        Debug.DrawRay(body.position, body.forward * 1000f, Color.blue, Time.deltaTime);

        if (hasHit && hit.transform.tag == "Environment")
        {
            print("raycast hit wall");
            if (hit.distance <= threshold)
            {
                print("hit wall less than threshold");
                TakeNinetyUp(hit);
            }
        }


    }

    private void TakeNinetyUp(RaycastHit target)
    {
        navMeshAgent.enabled = false;


        startPos = transform.position;
        endPos = target.point;

        startRot = transform.rotation;
        endRot = Quaternion.Euler(Vector3.up);


        transitioning = true;
        startTime = Time.time;
    }
}
