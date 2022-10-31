using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rb;
    private Vector3 _normal;
    private float _horizontalAxis;
    private float _verticalAxis;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Move(new Vector3(_verticalAxis, 0, -_horizontalAxis));
    }

    public void Move(Vector3 direction)
    {
        var directionAlongSurface = Project(direction.normalized);
        var offset = directionAlongSurface * (_speed * Time.deltaTime);

        _rb.MovePosition(_rb.position + offset);
    }

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            var normal = contact.normal;
            if (IsGround(normal))
            {
                _normal = normal;
                break;
            }
        }
    }

    private bool IsGround(Vector3 normal)
    {
        if (normal.x > 0.5) return false;
        if (normal.z > 0.5) return false;
        if (normal.y < 0.1) return false;

        return true;
    }


}
