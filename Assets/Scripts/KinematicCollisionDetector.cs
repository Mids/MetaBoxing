using System.Collections;
using OVR;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MetaBoxing
{
    public class KinematicCollisionDetector : MonoBehaviour
    {
        public SoundFXRef sfxRef;
        public GameObject vfxRef;

        public float score = 0;
        public TextMeshPro tmp;

        public bool isMyself = false;

        private Vector3 _contactPoint;
        private readonly float _recoveryTime = 1f;


        private void Start()
        {
            isMyself = gameObject.GetComponentInParent<PhotonView>().IsMine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            var view = other.gameObject.GetComponentInParent<PhotonView>();

            if (view != default && view.IsMine != isMyself)
            {
                score += 5;

                if (score < 50)
                    tmp.text = $"{(int) score}";
                else
                    tmp.text = "You win!";

                _contactPoint = other.ClosestPoint(transform.position);
                PlayHitFX();

                if (isMyself) return;

                var xrController = other.gameObject.GetComponent<KinematicController>()?.controller;
                if (xrController != default)
                    xrController.SendHapticImpulse(1f, 0.1f);
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