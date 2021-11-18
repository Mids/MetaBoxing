using System.Collections.Generic;
using System.Linq;
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
        public List<Transform> colliderTransforms;

        private AgentCollisionDetector _cd;

        public BoxingAgent opponent;

        public float myScore = 0f;
        private float _myCumScore = 0f;
        public float myNegScore = 0f;
        private float _myCumNegScore = 0f;

        public TextMeshPro tmp;
        public int steps = 0;

        private Quaternion _rootInv = default;
        private Vector3 _rootPos = default;

        public override void Initialize()
        {
            _cd = GetComponentInChildren<AgentCollisionDetector>();
            colliderTransforms = GetComponentsInChildren<Collider>().Select(p => p.transform).ToList();
            if (opponent != default) opponent.opponent = this;
            _abList = new List<ArticulationBody> {hipsAB, upperArmLAB, lowerArmLAB, upperArmRAB, lowerArmRAB};
            _rootInv = Quaternion.Inverse(transform.rotation);
            _rootPos = transform.position;
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
            foreach (var ab in _abList)
            {
                if (ab.jointPosition.dofCount >= 1)
                    sensor.AddObservation(ab.jointPosition[0]);
                if (ab.jointPosition.dofCount >= 2)
                    sensor.AddObservation(ab.jointPosition[1]);
                if (ab.jointPosition.dofCount >= 3)
                    sensor.AddObservation(ab.jointPosition[2]);
            }

            foreach (var t in colliderTransforms)
                sensor.AddObservation(_rootInv * (t.position - _rootPos));

            foreach (var t in opponent.colliderTransforms)
                sensor.AddObservation(_rootInv * (t.position - _rootPos));

            sensor.AddObservation((opponent.colliderTransforms[0].position - colliderTransforms[1].position).magnitude);
            sensor.AddObservation((opponent.colliderTransforms[0].position - colliderTransforms[2].position).magnitude);
            sensor.AddObservation((opponent.colliderTransforms[1].position - colliderTransforms[0].position).magnitude);
            sensor.AddObservation((opponent.colliderTransforms[2].position - colliderTransforms[0].position).magnitude);
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
            if (myNegScore > 0)
            {
                // AddReward(myNegScore / -20f);
                _myCumNegScore += myNegScore;
                myNegScore = 0;
                tmp.text = $"{(int) _myCumNegScore}";
            }

            if (myScore > 0)
            {
                _myCumScore += myScore;
                AddReward(myScore * (_myCumScore / (_myCumScore + _myCumNegScore)) / 10f);
                myScore = 0;
            }

            if (_myCumScore > 50)
            {
                AddReward(1f);
                EndEpisode();
            }
            else if (_myCumNegScore > 50)
            {
                EndEpisode();
            }

            ++steps;
            AddReward(-0.0005f);
            if (steps > 2000)
                EndEpisode();
            RequestDecision();
        }
    }
}