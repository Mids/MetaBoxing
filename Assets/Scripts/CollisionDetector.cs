using System.Collections;
using OVR;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MetaBoxing
{
    public class CollisionDetector : MonoBehaviour
    {
        public SoundFXRef sfxRef;
        public GameObject vfxRef;

        public float score = 0;
        public TextMeshPro tmp;

        public ArticulationBody body;
        public bool isMyself = false;

        private Vector3 _contactPoint;
        private float _defaultStiff;
        private readonly float _recoveryTime = 1f;


        private void Start()
        {
            isMyself = gameObject.GetComponentInParent<PhotonView>().IsMine;
            body = GetComponent<ArticulationBody>();
            _defaultStiff = body.xDrive.stiffness;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            var view = other.gameObject.GetComponentInParent<PhotonView>();

            if (view != default && view.IsMine != isMyself)
            {
                var ab = other.gameObject.GetComponent<ArticulationBody>();
                score += (ab.velocity - body.velocity).magnitude;

                if (score < 50)
                    tmp.text = $"{(int) score}";
                else
                    tmp.text = "You win!";

                var xDrive = body.xDrive;
                xDrive.stiffness = 0;
                body.xDrive = xDrive;

                var zDrive = body.zDrive;
                zDrive.stiffness = 0;
                body.zDrive = zDrive;

                _contactPoint = other.GetContact(0).point;
                PlayHitFX();

                if (isMyself) return;

                var xrController = other.gameObject.GetComponent<ABController>()?.controller;
                if (xrController != default)
                    xrController.SendHapticImpulse(1f, 0.1f);
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

        private void PlayHitFX()
        {
            StartCoroutine(PlayVFXAndDestroy());
            sfxRef.PlaySound();
        }

        private IEnumerator PlayVFXAndDestroy()
        {
            var vfx = Instantiate(vfxRef, _contactPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Destroy(vfx);
        }
    }
}