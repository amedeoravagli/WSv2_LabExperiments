using UnityEngine;

public class MoveSkater : Player
{
    private float _speed = 7.0f;
    private float _factor = .0f;
    private float _factorStep = 0.05f;
    private float _speedRot = 2.0f;
    private Vector3 _dir = Vector3.zero;
    private CharacterController _target;
    private Quaternion motionRotation = Quaternion.identity;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _target = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 motionVector = Vector3.zero;
        
        //transform.Rotate(Vector3.up, horizontalOrientation * _speedRot);
        if (_gyroscope.isActiveAndEnabled)
        {
            motionRotation = horizontalOrientation;
            //_target.transform.localEulerAngles = horizontalOrientation * Vector3.up;
           // _target.transform.localRotation *= Quaternion.Euler(horizontalOrientation * Vector3.up); 
        }else
        {
            _target.transform.Rotate(_speedRot * horizontalOrientation.eulerAngles.y * Vector3.up);

        }

        motionVector = movePlayer();
        motionVector = ApplyGravity(motionVector);
        // rotation _target
        //motionVector = Quaternion.Euler(_speedRot * horizontalOrientation * Vector3.up) * motionVector;
        _target.Move(motionVector);
    }

    protected override Vector3 movePlayer()
    {
        _factorStep = -1 * Mathf.Abs(_factorStep);
        if(isMovingFB || isMovingLR){
            if (_target.isGrounded){
                _factorStep = -1 * _factorStep;
                _dir = new Vector3(horizontalMove, 0, verticalMove);
                _dir = transform.localRotation * _dir;
                _dir = motionRotation * _dir;
            }
        }
        _factor += _factorStep;
        if (_factor < 0) _factor = 0; else if(_factor > 1) _factor = 1;

        return _factor * _speed * Time.deltaTime * _dir;
    }

    private Vector3 ApplyGravity(Vector3 moveDirection)
    {
        moveDirection.y -= 9.81f * Time.deltaTime;
        return moveDirection;
    }
}
