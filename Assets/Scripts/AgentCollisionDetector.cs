using UnityEngine;

namespace MetaBoxing
{
    public class AgentCollisionDetector : MonoBehaviour
    {
        private ArticulationBody _body;
        private BoxingAgent _myself;

        // Start is called before the first frame update
        private void Start()
        {
            _body = GetComponent<ArticulationBody>();
            _myself = GetComponentInParent<BoxingAgent>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Agent")) return;

            if (other.gameObject.GetComponentInParent<BoxingAgent>() == _myself) return;

            var ab = other.gameObject.GetComponent<ArticulationBody>();
            var score = other.impulse.magnitude / 30;
            // score *= score * score;


            if (score > 1)
            {
#if UNITY_EDITOR
                print($"{score} for {_myself.opponent.name}");
#endif
                // score *= score;
                _myself.opponent.myScore += score;
                _myself.myNegScore += score;
            }


            var xDrive = _body.xDrive;
            xDrive.stiffness = 0;
            _body.xDrive = xDrive;

            var zDrive = _body.zDrive;
            zDrive.stiffness = 0;
            _body.zDrive = zDrive;
        }
    }
}