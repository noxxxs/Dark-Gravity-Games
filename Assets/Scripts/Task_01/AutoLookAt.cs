using UnityEngine;

public class AutoLookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField, Min(0f)] private float _rotationSpeed;

    public bool LockYAxis = false;

    private Transform _transform;
    private Vector3 _direction;
    private Quaternion _targetRotation;
    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        _direction = _target.position - _transform.position;
        if (_direction.sqrMagnitude < 0.0001f) 
            return;

        if (LockYAxis) 
            _direction.y = 0f;

        _targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Lerp(_transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }

}
