using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MetaBoxing
{
    public class BoxingAgent : Agent
    {
        public ArticulationBody hipsAB;
        public ArticulationBody upperArmLAB;
        public ArticulationBody lowerArmLAB;
        public ArticulationBody upperArmRAB;
        public ArticulationBody lowerArmRAB;
        private List<ArticulationBody> _abList;

        private AgentCollisionDetector _cd;

        public BoxingAgent opponent;

        public float myScore = 0f;
        private float _myCumScore = 0f;
        public float myNegScore = 0f;
        private float _myCumNegScore = 0f;

        public TextMeshPro tmp;
        public int steps = 0;

        public override void Initialize()
        {
            _cd = GetComponentInChildren<AgentCollisionDetector>();
            if (opponent != default) opponent.opponent = this;
            _abList = new List<ArticulationBody> {hipsAB, upperArmLAB, lowerArmLAB, upperArmRAB, lowerArmRAB};
        }

        public override void OnEpisodeBegin()
        {
            myScore = 0f;
            _myCumScore = 0f;
            myNegScore = 0f;
            _myCumNegScore = 0f;
            tmp.text = $"{(int) _myCumScore}";

            steps = 0;

            foreach (var ab in _abList)
            {
                ab.jointPosition = new ArticulationReducedSpace(0, 0, 0);
                ab.jointVelocity = new ArticulationReducedSpace(0, 0, 0);
            }
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
            // TODO:
            if (myScore > 0)
            {
                AddReward(myScore / 10f);
                _myCumScore += myScore;
                myScore = 0;
                tmp.text = $"{(int) _myCumScore}";
            }

            if (myNegScore > 0)
            {
                AddReward(myNegScore / -20f);
                _myCumNegScore += myNegScore;
                myNegScore = 0;
            }

            if (_myCumScore > 50)
            {
                AddReward(1f);
                // print($"{GetCumulativeReward()} and win");
                EndEpisode();
            }
            else if (_myCumNegScore > 50)
            {
                AddReward(-1f);
                // print($"{GetCumulativeReward()} and lose");
                EndEpisode();
            }

            ++steps;
            AddReward(-0.001f);
            if (steps > 2000)
                EndEpisode();
            RequestDecision();
        }
    }
}