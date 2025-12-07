using UnityEngine;

namespace PlayerDir
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        
        private Transform _transform;
        private Vector3 _startPosition;
        private float _offset;

        private void Awake()
        {
            _transform = transform;
            _startPosition = _transform.position;
            _offset = (_target.position - _startPosition).z;
        }

        private void LateUpdate()
        {
            _startPosition.x = _target.position.x;
            _transform.position = _startPosition;
        }
    }
}
