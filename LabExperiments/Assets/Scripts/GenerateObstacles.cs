using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour
{
    [SerializeField] private List<GameObject> _obstacles;
    [SerializeField] private GameObject _coin;
    [SerializeField] private AnimationClip _floating;
    private BoxCollider _floor;
    private int num_obstacles = 3;

    //private Transform _obstacleTransform;

    // Start is called before the first frame update
    void Start()
    {
        int num_GO = _obstacles.Count;
        //Debug.Log("num_GO = " + num_GO);
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        SphereCollider _coinCollider = _coin.GetComponent<SphereCollider>();
        //Animation _coinAnim = _coin.GetComponent<Animation>();
        AnimationCurve x_curve = new AnimationCurve();
        //AnimationCurve y_curve = new AnimationCurve();
        AnimationCurve z_curve = new AnimationCurve();
        float _coinRadiusArea = 2*_coinCollider.radius;
        _floor = GetComponentInParent<BoxCollider>();
        float x_min = -_floor.bounds.size.x / 2;
        float x_max = -x_min;
    
        float z_min = -_floor.bounds.size.z / 2;
        float z_max = -z_min;
        float x_witdh = _floor.bounds.size.x;
        
        for (int i = 0; i < num_obstacles; i++)
        {
            int i_obs = Random.Range(0, num_GO+1);
          

            //Debug.Log("name GO " + _obstacles[i_obs].name + " i_obs = " + i_obs + " y = " + y);
            z = Random.Range(z_min, z_max);
            switch (i)
            {
                case 0:
                    x = Random.Range(x_min, x_min + (x_witdh / 3));
                    break;
                case 1:
                    x = Random.Range(-x_witdh/6, x_witdh/6);
                    break;
                case 2:
                    x = Random.Range(x_max - (x_witdh / 3), x_max);
                    break;
                default:
                    break;
            }
            
            //_obstacleTransform = transform;
            if(i_obs == num_GO) //coins
            {
                for (int j = 0; j < 3 ; j++)
                {
                    z += _coinRadiusArea; 
                    
                    /*x_curve.AddKey(new Keyframe(0.0f, x));
                    z_curve.AddKey(new Keyframe(0.0f, z));
                    //y_curve.AddKey(new Keyframe(0.0f, 0.2f));
                    //y_curve.AddKey(new Keyframe(1.0f, 0.5f));
                    //y_curve.AddKey(new Keyframe(2.0f, 0.2f));
                    //y_curve.AddKey(new Keyframe(3.0f, 0.5f));
                    //y_curve.AddKey(new Keyframe(4.0f, 0.2f));
                    
                    _floating.SetCurve("", typeof(Transform), "localPosition.x", x_curve);
                    //_floating.SetCurve("", typeof(Transform), "localPosition.x", x_curve);
                    _floating.SetCurve("", typeof(Transform), "localPosition.z", z_curve);
                    
                    _coinAnim.AddClip(_floating, "_codeFloating");*/
                    //_obstacles[i_obs].transform.localScale = new Vector3(1, 1, 1);
                    //_obstacles[i_obs].transform.rotation = Quaternion.identity;
                    GameObject _c = Instantiate(_coin, transform);
                    _c.transform.localPosition = new Vector3(x, 0.2f, z);
                }
            }
            else
            {
                
                //_obstacles[i_obs].transform.rotation = Quaternion.identity;
                GameObject _obs = Instantiate(_obstacles[i_obs], transform);
                _obs.transform.localPosition = new Vector3(x, y, z);
                _obs.transform.localScale = new Vector3(1, 1, 1);
            }
            
        }
    }

    
}
