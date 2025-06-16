using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{
    public Transform target;
    Vector3 velocity = Vector3.zero;

    public float Time;

    public Vector3 posOffset;   
    private void Move()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void LateUpdate()
    {
        Vector3 cordiPosition = target.position + posOffset;
        transform.position = Vector3.SmoothDamp(transform.position, cordiPosition, ref velocity, Time);
    }
}
