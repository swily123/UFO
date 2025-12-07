using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class Player : MonoBehaviour
{
    [SerializeField] private Engine _engine;
    [SerializeField] private float _constantForcePower;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private ConstantForce _constantForce;

    private void Awake()
    {
        _transform = transform;
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _constantForce = GetComponent<ConstantForce>();

        _engine.Initialize(_rigidbody);
    }

    private void FixedUpdate()
    {
        _constantForce.force = Vector3.right * (_playerInput.Controls.x * -_constantForcePower) + Physics.gravity * _rigidbody.mass;
    }

    private void Update()
    {
        bool isVerticalAxisActive = Mathf.Approximately(_playerInput.Controls.y, 0) == false;
        
        if (isVerticalAxisActive)
        {
            _engine.SetAltitude(_engine.GetCurrentAltitude());
            _engine.SetOverrideControls(_playerInput.Controls.y);
        }

        _engine.IsOverrided = isVerticalAxisActive;
    }
}