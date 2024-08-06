using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour
{
    public GameObject _obstacle;
    public int num_obstacles = 4;
    private Transform _obstacleTransform;
    // Start is called before the first frame update
    void Start()
    {
        float x = 0.0f;
        float y = 0.5f;
        float z = 0.0f;
        for (int i = 0; i < num_obstacles; i++)
        {
            switch (i)
            {
                case 0:
                    x = Random.Range(0.0f, 5.0f);
                    z = Random.Range(0.0f, 5.0f);
                    break;
                case 1:
                    x = Random.Range(0.0f, -5.0f);
                    z = Random.Range(0.0f, 5.0f);
                    break;
                case 2:
                    x = Random.Range(0.0f, -5.0f);
                    z = Random.Range(0.0f, -5.0f);
                    break;
                case 3:
                    x = Random.Range(0.0f, 5.0f);
                    z = Random.Range(0.0f, -5.0f);
                    break;
                default:
                    break;
            }
            
            _obstacleTransform = transform;
            _obstacle.transform.localPosition = new Vector3(x, y, z);
            _obstacle.transform.localScale = new Vector3(1, 1, 1);
            _obstacle.transform.rotation = Quaternion.identity;
            Instantiate(_obstacle, _obstacleTransform);
        }
    }

    
}
