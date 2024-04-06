using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float Speed;
    
    private Vector3 Offset;

    private void Awake()
    {
        Offset = transform.position;
    }

    void Update()
    {
        Vector3 delta = Target.position + Offset - transform.position;
        transform.Translate(Time.deltaTime * Speed * delta, Space.World);
    }
}