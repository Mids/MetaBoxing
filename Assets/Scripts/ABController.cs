using UnityEngine;
using UnityEngine.Assertions;

namespace MetaBoxing
{
    public class ABController : MonoBehaviour
    {
        public Transform kinematicBody;

        private Quaternion _defaultRot;
        private Quaternion _inverseDefaultRot;
        private ArticulationBody _ab;

        private void Start()
        {
            Assert.IsNotNull(kinematicBody, "Set the target");
            _defaultRot = transform.localRotation;
            _inverseDefaultRot = Quaternion.Inverse(_defaultRot);
            _ab = GetComponent<ArticulationBody>();
            Assert.IsNotNull(_ab, "You need a ArticulationBody component");
        }

        private void FixedUpdate()
        {
            var targetAngle = (_inverseDefaultRot * kinematicBody.localRotation).eulerAngles;

            if (_ab.linearLockX != ArticulationDofLock.LockedMotion)
            {
                targetAngle.x = Mathf.DeltaAngle(0, targetAngle.x);
                var drive = _ab.xDrive;
                if (_ab.linearLockX == ArticulationDofLock.LimitedMotion)
                    targetAngle.x = Mathf.Clamp(targetAngle.x, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.x;
                _ab.xDrive = drive;
            }

            if (_ab.linearLockY != ArticulationDofLock.LockedMotion)
            {
                targetAngle.y = Mathf.DeltaAngle(0, targetAngle.y);
                var drive = _ab.yDrive;
                if (_ab.linearLockY == ArticulationDofLock.LimitedMotion)
                    targetAngle.y = Mathf.Clamp(targetAngle.y, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.y;
                _ab.yDrive = drive;
            }

            if (_ab.linearLockZ != ArticulationDofLock.LockedMotion)
            {
                targetAngle.z = Mathf.DeltaAngle(0, targetAngle.z);
                var drive = _ab.zDrive;
                if (_ab.linearLockZ == ArticulationDofLock.LimitedMotion)
                    targetAngle.z = Mathf.Clamp(targetAngle.z, drive.lowerLimit, drive.upperLimit);
                drive.target = targetAngle.z;
                _ab.zDrive = drive;
            }
        }
    }
}