using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This class contains and controlls all the different behaviours for all 3 stages of the experiment.
/// </summary>
public class CubeBehaviour : MonoBehaviour
{
    /// <summary>
    /// Controlls the cube behaviour for the meeting stage (subtask a)
    /// </summary>
    /// <param name="controller">of the cube where this behaviour is invoked</param>
    /// <param name="forceFactor">factor to scale the force on the cube</param>
    public static void MeetingBehaviour(CubeController controller, float forceFactor)
    {
        //do nothing, just report
        ReportData(controller, "meeting");
    }

    /// <summary>
    /// Controlls the cube behaviour for the break-up stage (subtask b)
    /// </summary>
    /// <param name="controller">of the cube where this behaviour is invoked</param>
    /// <param name="forceFactor">direction and factor of breakup</param>
    public static void BreakupBehaviour(CubeController controller, float forceFactor)
    {
        Rigidbody body = controller.Body;
        float forceX = 0f;
        float distance = LabManager.Instance.GetDistanceBetweenCubes();

        //max lenght of spring
        distance = distance <= 5f ? distance : 0f;

        forceX = -forceFactor * distance;
        body.AddForce(new Vector3(forceX, 0f, 0f));

        ReportData(controller, "break");
    }

    /// <summary>
    /// Controlls the cube behaviour for the drifting stage (subtask b)
    /// </summary>
    /// <param name="controller">of the cube where this behaviour is invoked</param>
    /// <param name="forceFactor">not used for this calc but needed from interface</param>
    public static void DriftingBehaviour(CubeController controller, float forceFactor)
    {
        Rigidbody body = controller.Body;

        //friction
        float my = 0.1f;
        float force_friction = my * body.mass * Physics.gravity.magnitude;
        body.AddForce(-force_friction * Vector3.Normalize(body.velocity));

        //centrifugal
        float len_force_cent = body.mass * (float)Math.Pow(body.velocity.magnitude, 2) / 5f;
        Vector3 force_cent = len_force_cent * LabManager.Instance.GetDirectionToPole();
        body.AddForce(force_cent);

        ReportData(controller, "drift");
    }

    /// <summary>
    /// Controlls the cube behaviour for the drifting stage (subtask c)
    /// </summary>
    /// <param name="controller">of the cube where this behaviour is invoked</param>
    /// <param name="forceFactor">not used for this calc but needed from interface</param>
    public static void FallingBehaviour(CubeController controller, float forceFactor)
    {
        ReportData(controller, "fall");
    }

    private static void ReportData(CubeController controller, string stage)
    {
        Rigidbody body = controller.Body;
        List<float> data = new List<float>() {  controller.TimeStep,
                                                body.position.x,
                                                body.position.y,
                                                body.position.z,
                                                body.velocity.x,
                                                body.velocity.y,
                                                body.velocity.z };

        LabManager.Instance.Reporter.ReportData(data, body.transform.name, stage);
    }
}