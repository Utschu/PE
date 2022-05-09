using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class controlls the actions of the cubes
/// </summary>
public class CubeController : MonoBehaviour
{
    public int directionOfTravel;
    public float springConstant; // N/m

    public delegate void Behaviour(CubeController controller, float forceFactor);
    public Behaviour behaviour;

    public float TimeStep { get; private set; } // s
    public Rigidbody Body { get; private set; }

    void Start()
    {
        Body = GetComponent<Rigidbody>();
        behaviour += CubeBehaviour.MeetingBehaviour;
    }

    void FixedUpdate()
    {
        behaviour?.Invoke(this, directionOfTravel * springConstant);
        TimeStep += Time.deltaTime;
    }
}
