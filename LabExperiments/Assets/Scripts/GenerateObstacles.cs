using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour
{
    [SerializeField] private List<GameObject> _obstacles;
    private int num_obstacles = 3;
    private Transform _obstacleTransform;
    // Start is called before the first frame update
    void Start()
    {
        int num_GO = _obstacles.Count;
        Debug.Log("num_GO = " + num_GO);
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        for (int i = 0; i < num_obstacles; i++)
        {
            int i_obs = Random.Range(0, num_GO);
          

            Debug.Log("name GO " + _obstacles[i_obs].name + " i_obs = " + i_obs + " y = " + y);
            z = Random.Range(-4.0f, 4.0f);
            switch (i)
            {
                case 0:
                    x = Random.Range(-4.0f, -1.5f);
                    break;
                case 1:
                    x = Random.Range(-1.25f, 1.25f);
                    break;
                case 2:
                    x = Random.Range(1.5f, 4.0f);
                    break;
                default:
                    break;
            }
            
            _obstacleTransform = transform;
            _obstacles[i_obs].transform.localPosition = new Vector3(x, y, z);
            _obstacles[i_obs].transform.localScale = new Vector3(1, 1, 1);
            //_obstacles[i_obs].transform.rotation = Quaternion.identity;
            Instantiate(_obstacles[i_obs], _obstacleTransform);
        }
    }

    
}
