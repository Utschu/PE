using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

/// <summary>
/// This class acts as a game master object, meaning it controll the flow and action of this experiment
/// </summary>
public class LabManager : MonoBehaviour
{
    private Rigidbody heavyCubeBody;
    private CubeController heavyCubeController;
    private Rigidbody lightCubeBody;
    private CubeController lightCubeController;

    private GameObject pole;
    private GameObject hole;

    public static LabManager Instance { get; private set; }
    public DataReporter Reporter { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Reporter = new DataReporter();
    }

    void Start()
    {
        GameObject obj = GameObject.Find("HeavyCube");
        heavyCubeBody = obj.GetComponent<Rigidbody>();
        heavyCubeController = obj.GetComponent<CubeController>();

        obj = GameObject.Find("LightCube");
        lightCubeBody = obj.GetComponent<Rigidbody>();
        lightCubeController = obj.GetComponent<CubeController>();

        pole = GameObject.Find("Pole");
        hole = GameObject.Find("Hole");

        InitMeetingStage();
    }

    void OnApplicationQuit()
    {
        CSVWriter.WriteToCSV(Reporter);
    }

    /// <summary>
    /// Initializes the meeting stage of the experiment (subtask a)
    /// </summary>
    private void InitMeetingStage()
    {
        lightCubeBody.freezeRotation = true;
        heavyCubeBody.freezeRotation = true;

        lightCubeBody.AddForce(new Vector3(-5000f, 0f, 0f));
    }

    /// <summary>
    /// Initializes the break-up stage of the experiment (subtask a)
    /// </summary>
    public void InitBreakupStage()
    {
        lightCubeController.directionOfTravel = -1;
        lightCubeController.behaviour = null;
        lightCubeController.behaviour += CubeBehaviour.BreakupBehaviour;

        heavyCubeBody.freezeRotation = false;
        heavyCubeController.behaviour = null;
        heavyCubeController.behaviour += CubeBehaviour.BreakupBehaviour;
    }

    /// <summary>
    /// Helper method for breakup stage
    /// </summary>
    /// <returns>distance between cubes</returns>
    public float GetDistanceBetweenCubes()
    {
        return Vector3.Distance(heavyCubeBody.transform.position, lightCubeBody.transform.position);
    }

    /// <summary>
    /// Initializes the drifting stage of the experiment (subtask b)
    /// </summary>
    public void InitDriftingStage()
    { 
        lightCubeController.behaviour = null;
        lightCubeController.behaviour += CubeBehaviour.DriftingBehaviour;
    }

    /// <summary>
    /// Helper method for drifting stage
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirectionToPole()
    {
        return Vector3.Normalize(pole.transform.position - lightCubeBody.position);
    }


    /// <summary>
    /// Initializes the falling stage of the experiment (subtask c)
    /// </summary>
    public void InitFallingStage()
    {
        heavyCubeController.behaviour = null;
        heavyCubeController.behaviour += CubeBehaviour.FallingBehaviour;

        var heavyCubeCollider = GameObject.Find("HeavyCube").GetComponent<BoxCollider>();

        float h = 5f + (heavyCubeCollider.size.y / 2);

        //formula of throw distance with alpha = 0
        float posX = heavyCubeBody.velocity.magnitude * (float)Math.Cos(0) * 
            ((float)Math.Sqrt(2 * Physics.gravity.magnitude * h) / Physics.gravity.magnitude);

        float real_posX = heavyCubeBody.position.x - heavyCubeCollider.size.x - posX;
        hole.transform.position = new Vector3(real_posX, -h, 0);
    }
}