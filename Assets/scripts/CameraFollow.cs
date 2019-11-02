using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraPosition;
    private Transform target;
    [SerializeField] private Vector3 offset;
    [Range(0,1)][SerializeField] private float smoothSpeed = 0.125f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {   
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothing = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothing;
        transform.LookAt(smoothing);
    }
}
