using System;
using UnityEngine;
using Utils;

public class Engine : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _spherecastRadius;
    [SerializeField] private float _maxDistance;
    
    [SerializeField] private float _maxForce;
    [SerializeField] private float _damping;
    
    private Transform _transform;
    private Rigidbody _targetBody;
    private float _springSpeed;
    private float _oldDistance;
    [SerializeField] private float _altitude;
    
    public void Initialize(Rigidbody targetBody)
    {
        _transform = transform;
        _targetBody = targetBody;
    }

    private void FixedUpdate()
    {
        if (_targetBody == null)
            return;

        var forward = _transform.forward;

        Lift(forward);
    }

    private void Lift(Vector3 forward)
    {
        if (Physics.SphereCast(_transform.position, _spherecastRadius, forward, out RaycastHit hitInfo,  _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            var distance = hitInfo.distance;
            
            _springSpeed = (distance - _oldDistance) * Time.fixedTime;
            _springSpeed = Mathf.Max(_springSpeed, 0f);
            _oldDistance = distance;
            
            var minForceHeight = _altitude + 1f;
            var maxForceHeight = _altitude - 1f;
            distance = Mathf.Clamp(distance, maxForceHeight, minForceHeight);
            
            var forceFactor = distance.Remap(maxForceHeight, minForceHeight, _maxForce, 0);
            _targetBody.AddForce(-forward * Mathf.Max(forceFactor - _springSpeed * _maxForce * _damping, 0), ForceMode.Force);
            
            
        }
    }
}
