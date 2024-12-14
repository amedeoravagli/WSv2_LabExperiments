using UnityEngine;
using UnityEngine.InputSystem;

public class HMDMotionControl : MonoBehaviour
{
    [SerializeField] private InputActionReference hmdPositionReference; 
    private Vector3 oldPos;
    private Vector3 centerPosition;

    public float threshold = 25;
    public float horizontalMove = 0;
    public float verticalMove = 0;

    //private bool isReady = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        oldPos = hmdPositionReference.action.ReadValue<Vector3>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = hmdPositionReference.action.ReadValue<Vector3>();
        // valuate a motion only if ds/dt > threshold 
        float ds = Vector3.Angle(centerPosition, newPos);
        if(ds>threshold)
        {
            Vector3 directionVector = new(newPos.x, 0, newPos.z);
            directionVector.Normalize();
            horizontalMove = directionVector.x;
            verticalMove = directionVector.z;
        }

        // temporal mean on centerPosition: not needed for an experiment of 60 seconds
    }

    public void SaveCurrentPositionAsCenter()
    {

    }
}
