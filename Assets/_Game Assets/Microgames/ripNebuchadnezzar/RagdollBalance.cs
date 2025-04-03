using UnityEngine;

namespace _Game_Assets.Microgames.ripNebuchadnezzar
{
    public class RagdollBalance : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float balanceForce;
        [SerializeField] private float targetAngle;

        void FixedUpdate()
        {
            float angleDifference = Mathf.DeltaAngle(rb.rotation, targetAngle);
            float torque = angleDifference * balanceForce;
            rb.AddTorque(torque);
        }
        
        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            brokenJoint.enabled = false;
            enabled = false;
        }
    }
}
