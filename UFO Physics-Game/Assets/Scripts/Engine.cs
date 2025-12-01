using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _spherecastRadius;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _maxForce;
    
    private Transform _transform;
    private Rigidbody _targetBody;
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
            
            _targetBody.AddForce(-forward * (_maxForce * Mathf.Clamp01(1-distance/10f)), ForceMode.Force);
        }
    }
}
