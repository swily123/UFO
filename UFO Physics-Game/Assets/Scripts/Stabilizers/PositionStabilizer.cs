using UnityEngine;

namespace Stabilizers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PositionStabilizer : MonoBehaviour
    {
        [SerializeField] private float _stabilizerForce;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(Vector3.forward * (-_rigidbody.position.z * _stabilizerForce), ForceMode.Force); //TODO добавить damping
        }
    }
}