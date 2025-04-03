using UnityEngine;

namespace _Game_Assets.Microgames.ripNebuchadnezzar
{
    public class RagdollBalance : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float balanceForce;
        [SerializeField] private float targetAngle;

        private MoveRagdoll moveRagdoll;
        
        private void Start() => moveRagdoll = GetComponentInParent<MoveRagdoll>();

        void FixedUpdate()
        {
            float angleDifference = Mathf.DeltaAngle(rb.rotation, targetAngle);
            float torque = angleDifference * balanceForce;
            rb.AddTorque(torque);
        }

        public void ForceDisable()
        {
            if (TryGetComponent(out Joint2D joint2D))
            {
                joint2D.enabled = false;
            }
            
            enabled = false;
        }
        
        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            brokenJoint.enabled = false;
            enabled = false;
            
            moveRagdoll.JointBreak2D(rb);
        }
    }
}
