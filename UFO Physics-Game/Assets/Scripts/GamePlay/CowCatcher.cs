using PlayerDir;
using UnityEngine;

namespace GamePlay
{
    public class CowCatcher : MonoBehaviour
    {
        [SerializeField] private float _catchDistance;
        [SerializeField] private float _catchRadius;
        [SerializeField] private GameObject _effect;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _catchTime;

        private Transform _transform;
        private Transform _catchedCow;
        private float _catchTimer = float.MinValue;
        private bool _isCatchActionActive;
        private Vector3 _startCowPosition;
        private Vector3 _startCowScale;
        private PlayerInput _input;

        private void Awake()
        {
            _transform = transform;
        }
        
        private void Update()
        {
            if (_catchTimer > 0)
            {
                _catchTimer -= Time.deltaTime/_catchTime;

                if (_catchTimer <= 0)
                {
                    if (_catchedCow != null)
                    {
                        Destroy(_catchedCow.gameObject);
                        _catchedCow = null;
                        OnCatchReleased();
                    }
                }
            }

            if (_catchedCow != null)
                UpdateCowTransform();
        }

        private void FixedUpdate()
        {
            if (_isCatchActionActive == false) return;
            if (_catchedCow != null) return;

            var colliders = Physics.OverlapSphere(_transform.position + _transform.forward * _catchDistance, _catchRadius, _layerMask, QueryTriggerInteraction.Ignore);

            foreach (Collider col in colliders)
            {
                var cow = col.GetComponentInParent<Cow>();
                
                if (cow != null)
                {
                    cow.Catched();
                    _catchedCow = cow.transform;
                    _catchedCow.SetParent(_transform);
                    _startCowPosition = _catchedCow.localPosition;
                    _startCowScale = _catchedCow.localScale;
                    
                    _catchTimer = 1f;
                    break;
                }
            }
        }

        private void OnDisable()
        {
            UnsubscribeInput();
        }

        public void SetInput(PlayerInput input)
        {
            UnsubscribeInput();
            _input = input;
            input.CatchPressed += OnCatchPressed;
            input.CatchReleased += OnCatchReleased;
        }

        private void UnsubscribeInput()
        {
            if (_input != null)
            {
                _input.CatchReleased -= OnCatchPressed;
                _input.CatchPressed -= OnCatchReleased;
            }
        }
        
        private void UpdateCowTransform()
        {
            float t = Mathf.SmoothStep(0, 1, _catchTimer);
            
            _catchedCow.transform.localPosition = Vector3.Lerp(Vector3.zero, _startCowPosition, t);
            _catchedCow.transform.localScale = Vector3.Lerp(Vector3.zero, _startCowScale, t);
        }
        
        private void OnCatchReleased()
        {
            if (_catchedCow != null)
                return;
            
            SetEffect(false);
        }

        private void OnCatchPressed()
        {
            SetEffect(true);
        }

        private void SetEffect(bool value)
        {
            _isCatchActionActive = value;
            _effect.SetActive(value);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + transform.forward * _catchDistance, _catchRadius);
        }
    }
}