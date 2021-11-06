using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BoxingAgent : Agent
{
    public ArticulationBody hipsAB;
    public ArticulationBody upperArmLAB;
    public ArticulationBody lowerArmLAB;
    public ArticulationBody upperArmRAB;
    public ArticulationBody lowerArmRAB;

    public SphereCollider head;

    public override void Initialize()
    {
    }

    public override void OnEpisodeBegin()
    {
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var index = 0;

        hipsAB.xDrive = SetDriveTarget(hipsAB.xDrive, vectorAction[index++]);
        hipsAB.zDrive = SetDriveTarget(hipsAB.zDrive, vectorAction[index++]);
        
        upperArmLAB.xDrive = SetDriveTarget(upperArmLAB.xDrive, vectorAction[index++]);
        upperArmLAB.yDrive = SetDriveTarget(upperArmLAB.yDrive, vectorAction[index++]);
        upperArmLAB.zDrive = SetDriveTarget(upperArmLAB.zDrive, vectorAction[index++]);
        lowerArmLAB.xDrive = SetDriveTarget(lowerArmLAB.xDrive, vectorAction[index++]);
        
        upperArmRAB.xDrive = SetDriveTarget(upperArmRAB.xDrive, vectorAction[index++]);
        upperArmRAB.yDrive = SetDriveTarget(upperArmRAB.yDrive, vectorAction[index++]);
        upperArmRAB.zDrive = SetDriveTarget(upperArmRAB.zDrive, vectorAction[index++]);
        lowerArmRAB.xDrive = SetDriveTarget(lowerArmRAB.xDrive, vectorAction[index++]);
        
        if (index != vectorAction.Length) Debug.LogError($"{index} is smaller than {vectorAction.Length}");
    }

    public override void Heuristic(float[] actionsOut)
    {
        for (var i = 0; i < actionsOut.Length; i++) actionsOut[i] = Random.Range(-1f, 1f);
    }

    private ArticulationDrive SetDriveTarget(ArticulationDrive drive, float x)
    {
        var range = drive.upperLimit - drive.lowerLimit;
        var target = drive.lowerLimit + range / 2 * (x + 1);
        drive.target = target;
        return drive;
    }

    private void FixedUpdate()
    {
        RequestDecision();
    }
}