using PlayerDir;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class Cow : MonoBehaviour
    {
        [SerializeField] private float _jumpPower;
        [SerializeField] private GameObject _deadCowPrefab;
        [SerializeField] private float _minJumpTime = 1f;
        [SerializeField] private float _maxJumpTime = 2f;

        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private static readonly int Fly = Animator.StringToHash("Fly");
        private Animator _animator;
        private Rigidbody _rigidbody;
        private float _jumpTimer = 1;
        private bool _catched;
        
        public void Catched()
        {
            _catched = true;
            _animator.SetBool(Fly, true);
            _rigidbody.isKinematic = true;
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_catched == false)
                TryJump();
        }

        private void TryJump()
        {
            if (_jumpTimer > 0)
            {
                _jumpTimer -= Time.deltaTime;

                if (_jumpTimer < 0)
                {
                    Jump();
                    _jumpTimer = Random.Range(_minJumpTime, _maxJumpTime);
                }
            }
        }
        
        private void Jump()
        {
            _animator.SetTrigger(Jump1);
            _rigidbody.velocity = (Vector3.up + transform.forward) *  _jumpPower;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_catched) return;
            
            var attachedRigidbody = other.collider.attachedRigidbody;

            if (attachedRigidbody == null)
            {
                return;
            }
            
            if (attachedRigidbody.GetComponent<Player>() != null)
            {
                Instantiate(_deadCowPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}