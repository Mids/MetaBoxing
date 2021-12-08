using UnityEngine;

namespace MetaBoxing
{
    public class BodyLookAt : MonoBehaviour
    {
        public bool isPlayer = false;
        public Transform head;
        public Vector3 offset;

        private Vector3 _forward;
        private Vector3 _right;
        private Vector3 _up;

        public void Start()
        {
            if (!isPlayer) return;

            head = GameObject.Find("HeadTarget").transform;
            _forward = transform.forward;
            _right = transform.right;
            _up = transform.up;

            if (PlayerPrefs.GetInt("IS3PP") == 1)
                offset = new Vector3(0, -1, 1);
        }

        private void Update()
        {
            if (!isPlayer) return;

            var headPosition = head.position + offset;
            var dir = headPosition - transform.position;

            var projZ = Vector3.ProjectOnPlane(dir, _forward);
            var projX = Vector3.ProjectOnPlane(dir, _right);
            var rotZ = Vector3.SignedAngle(_up, projZ, _forward);
            var rotX = Vector3.SignedAngle(_up, projX, _right);

            transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, rotZ);
        }
    }
}