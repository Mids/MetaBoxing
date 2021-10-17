using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MetaBoxing
{
    public class CollisionDetector : MonoBehaviour
    {
        public float score = 0;
        public TextMeshPro tmp;

        public ArticulationBody body;
        public bool isMyself = false;

        private float _defaultStiff;
        private float _recoveryTime = 1f;
        

        private void Start()
        {
            isMyself = gameObject.GetComponentInParent<PhotonView>().IsMine;
            body = GetComponent<ArticulationBody>();
            _defaultStiff = body.xDrive.stiffness;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponentInParent<PhotonView>().IsMine != isMyself)
            {
                score += 1;
                tmp.text = "" + score;
                
                var xDrive = body.xDrive;
                xDrive.stiffness = 0;
                body.xDrive = xDrive;
                
                var zDrive = body.zDrive;
                zDrive.stiffness = 0;
                body.zDrive = zDrive;
            }
        }

        private void FixedUpdate()
        {
            if (body.xDrive.stiffness < _defaultStiff)
            {
                var xDrive = body.xDrive;
                xDrive.stiffness += _defaultStiff * Time.fixedDeltaTime / _recoveryTime;
                body.xDrive = xDrive;
                
                var zDrive = body.zDrive;
                zDrive.stiffness += _defaultStiff * Time.fixedDeltaTime / _recoveryTime;
                body.zDrive = zDrive;
            }
        }
    }
}