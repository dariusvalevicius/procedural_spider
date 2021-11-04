using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBehaviour : MonoBehaviour
{

    [SerializeField] SpiderAnimationController controller;

    [SerializeField] LegBehaviour altLegA;

    [SerializeField] LegBehaviour altLegB;

    float maxDistance;

    float legMoveTime;

    float stepHeight;
    Vector3 targetPosition;

    Vector3 initialPosition;

    bool moving;

    float startTime;


    // Start is called before the first frame update
    void Start()
    {
        maxDistance = controller.maxDistance;
        legMoveTime = controller.legMoveTime;
        stepHeight = controller.stepHeight;

        initialPosition = transform.position;
        targetPosition = transform.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        maxDistance = controller.maxDistance;
        legMoveTime = controller.legMoveTime;
        stepHeight = controller.stepHeight;


        if (!moving)
        {
            transform.position = initialPosition;
        }

        if (maxDistance < Vector3.Distance(transform.position, targetPosition) && !moving)
        {
            if (altLegA.IsMoving() || altLegB.IsMoving()) { return; }

            startTime = Time.time;
            moving = true;
        }

        if (moving)
        {
            MoveLeg();
            if (Vector3.Distance(transform.position, targetPosition) <= 0.03)
            {
                moving = false;
                initialPosition = transform.position;
            }
            //else if (Vector3.Distance(transform.position, targetPosition) > 2f)
            //{
            //print("resetting leg position");
            //transform.position = targetPosition;
            //moving = false;
            // do nothing
            //}
        }

    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void MoveLeg()
    {
        float moveFraction = (Time.time - startTime) / legMoveTime;
        Vector3 temp = Vector3.Lerp(initialPosition, targetPosition, moveFraction);
        float yOffset = Mathf.Sin(moveFraction * Mathf.PI) * stepHeight;
        Vector3 offset = new Vector3(0f, yOffset, 0f);

        transform.position = temp + offset;


    }

    public bool IsMoving()
    {
        return moving;
    }
}
