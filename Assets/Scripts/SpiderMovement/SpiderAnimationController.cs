using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationController : MonoBehaviour
{

    [Range(0f, 5f)] [SerializeField] public float maxDistance = 1f;
    [Range(0f, 1f)] [SerializeField] public float legMoveTime = 0.1f;

    [Range(0f, 2f)] [SerializeField] public float stepHeight = 0.2f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
