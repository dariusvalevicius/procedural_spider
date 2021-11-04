using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDown : MonoBehaviour
{
    [SerializeField] LegBehaviour legMovement;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, -transform.up, out hit);
        Debug.DrawRay(transform.position, -transform.up, Color.blue, Time.deltaTime);

        if (hasHit && legMovement != null)
        {
            legMovement.SetTargetPosition(hit.point);
        }
    }
}
