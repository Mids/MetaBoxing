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

        private void Start()
        {
            Assert.IsNotNull(kinematicBody, "Set the target");
            _ab = GetComponent<ArticulationBody>();
            Assert.IsNotNull(_ab, "You need a ArticulationBody component");

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

                var angle = Vector3.Angle(rootPos - elbowPos, handPos - elbowPos);
                var drive = _ab.xDrive;
                drive.target = 180f - angle;
                _ab.xDrive = drive;
                return;
            }

            var targetAngle = (Quaternion.Inverse(_ab.parentAnchorRotation) *
                               kinematicBody.localRotation * _ab.anchorRotation).eulerAngles;

            if (_ab.linearLockX != ArticulationDofLock.LockedMotion)
            {
                targetAngle.x = Mathf.DeltaAngle(0, targetAngle.x);
                var drive = _ab.xDrive;
                // if (!_ab.name.Equals("LowerArm_L"))
                targetAngle.x = Mathf.Clamp(targetAngle.x, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.x;
                _ab.xDrive = drive;
            }

            if (_ab.linearLockY != ArticulationDofLock.LockedMotion)
            {
                targetAngle.y = Mathf.DeltaAngle(0, targetAngle.y);
                var drive = _ab.yDrive;
                // if (!_ab.name.Equals("LowerArm_L"))
                targetAngle.y = Mathf.Clamp(targetAngle.y, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.y;
                _ab.yDrive = drive;
            }

            if (_ab.linearLockZ != ArticulationDofLock.LockedMotion)
            {
                targetAngle.z = Mathf.DeltaAngle(0, targetAngle.z);
                var drive = _ab.zDrive;
                // if (!_ab.name.Equals("LowerArm_L"))
                targetAngle.z = Mathf.Clamp(targetAngle.z, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.z;
                _ab.zDrive = drive;
            }
        }
    }
}