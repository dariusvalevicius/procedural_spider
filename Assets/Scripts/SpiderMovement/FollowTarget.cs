using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] public Transform target;
    [Range(0f, 10f)] [SerializeField] public float speed = 3f;

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        //navMeshAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && navMeshAgent.enabled)
        {
            navMeshAgent.destination = target.position;
        }


    }
}
