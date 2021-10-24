using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace MetaBoxing
{
    public class ABController : MonoBehaviour
    {
        public Transform kinematicBody;
        public XRBaseController controller;
        public bool isLeft = false;
        public bool isRight = false;

        private ArticulationBody _ab;
        private float _defaultAngle;

        private void Start()
        {
            Assert.IsNotNull(kinematicBody, "Set the target");
            _ab = GetComponent<ArticulationBody>();
            Assert.IsNotNull(_ab, "You need a ArticulationBody component");

            if (_ab.name.Equals("LowerArm_L") || _ab.name.Equals("LowerArm_R"))
            {
                var elbowPos = kinematicBody.position;
                var rootPos = kinematicBody.parent.position;
                var handPos = kinematicBody.GetChild(0).position;
                _defaultAngle = 180f - Vector3.Angle(rootPos - elbowPos, handPos - elbowPos);
            }

            if (isLeft)
                controller = GameObject.Find("LeftHand Controller").GetComponent<XRController>();
            if (isRight)
                controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        }

        private void FixedUpdate()
        {
            if (_ab.name.Equals("LowerArm_L") || _ab.name.Equals("LowerArm_R"))
            {
                var elbowPos = kinematicBody.position;
                var rootPos = kinematicBody.parent.position;
                var handPos = kinematicBody.GetChild(0).position;

                var angle = 180f - Vector3.Angle(rootPos - elbowPos, handPos - elbowPos) - _defaultAngle;
                // angle = Mathf.Max(0, angle);
                
                var drive = _ab.xDrive;
                drive.targetVelocity = (angle - drive.target) / Time.fixedDeltaTime;
                drive.target = angle;
                _ab.xDrive = drive;
                return;
            }

            var targetAngle = (Quaternion.Inverse(_ab.parentAnchorRotation) *
                               kinematicBody.localRotation * _ab.anchorRotation).eulerAngles;

            if (_ab.linearLockX != ArticulationDofLock.LockedMotion)
            {
                targetAngle.x = Mathf.DeltaAngle(0, targetAngle.x);
                var drive = _ab.xDrive;
                targetAngle.x = Mathf.Clamp(targetAngle.x, drive.lowerLimit, drive.upperLimit);
                drive.targetVelocity = (targetAngle.x - drive.target) / Time.fixedDeltaTime;
                drive.target = targetAngle.x;
                _ab.xDrive = drive;
            }

            if (_ab.linearLockY != ArticulationDofLock.LockedMotion)
            {
                targetAngle.y = Mathf.DeltaAngle(0, targetAngle.y);
                var drive = _ab.yDrive;
                targetAngle.y = Mathf.Clamp(targetAngle.y, drive.lowerLimit, drive.upperLimit);
                drive.targetVelocity = (targetAngle.y - drive.target) / Time.fixedDeltaTime;
                drive.target = targetAngle.y;
                _ab.yDrive = drive;
            }

            if (_ab.linearLockZ != ArticulationDofLock.LockedMotion)
            {
                targetAngle.z = Mathf.DeltaAngle(0, targetAngle.z);
                var drive = _ab.zDrive;
                targetAngle.z = Mathf.Clamp(targetAngle.z, drive.lowerLimit, drive.upperLimit);
                drive.targetVelocity = (targetAngle.z - drive.target) / Time.fixedDeltaTime;
                drive.target = targetAngle.z;
                _ab.zDrive = drive;
            }
        }
    }
}