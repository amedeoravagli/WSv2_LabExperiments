using System.Collections.Generic;
using UnityEngine;

public struct AreaLimits{
    public float x_min;
    public float x_max;
    public float z_min;
    public float z_max;

}

public class GenerateObstacles : MonoBehaviour
{
    [SerializeField] private List<GameObject> _obstacles;
    [SerializeField] private GameObject _coin;
    //[SerializeField] private AnimationClip _floating;
    private BoxCollider _genArea;

    public SubtrackInfo info;

    //private Transform _obstacleTransform;

    private float BiggerCollider(List<GameObject> objects){
        

        float old_area = 0.0f;
        foreach(var obj in objects)
        {
            SphereCollider c = obj.GetComponentInChildren<SphereCollider>();
            if (c == null)
            {
                CapsuleCollider capsule = obj.GetComponentInChildren<CapsuleCollider>();
                if (old_area < capsule.radius)
                {
                    old_area = capsule.radius;
                    
                }
            }else{
                if (old_area < c.radius)
                {
                    old_area = c.radius;
                    
                }
            }  
            
        }
        return old_area;
    }

    private List<Vector2> SearchObstaclePositions(AreaLimits area, float curveBound, float obsRadius){
        List<Vector2> result = new();
        float x,z;
        float areaZSize = area.z_max - area.z_min;
        //float normCurveBound = curveBound/areaXSize;
        //float normObsRadius = obsRadius/areaXSize;
        int count = 0;
        while(count < info.numOfObstacles){
            x = UnityEngine.Random.Range(-1.0f, 1.0f);
            z = UnityEngine.Random.Range(0.0f, 1.0f);

            float x_maxLimitCurve = (info.coinsPath.Evaluate(z) * area.x_max) + curveBound + obsRadius;
            float x_minLimitCurve = (info.coinsPath.Evaluate(z) * area.x_max) - curveBound - obsRadius;
            
            if (x * area.x_max < x_minLimitCurve || x * area.x_max > x_maxLimitCurve)
            {
                result.Add(new(x * area.x_max, (z*areaZSize)-area.z_max));
                count++;
            }

        }
        
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        int num_GO = _obstacles.Count;
        float x = 0.0f;
        float z = 0.0f;
        SphereCollider _coinCollider = _coin.GetComponentInChildren<SphereCollider>();
        float _coinRadiusArea = 2*_coinCollider.radius;
        
        float biggerRadius = BiggerCollider(_obstacles);
        _genArea = GetComponent<BoxCollider>();

        float x_min = -_genArea.bounds.size.x / 2;
        float x_max = -x_min;
    
        float z_min = -_genArea.bounds.size.z / 2;
        float z_max = -z_min;
        //float x_witdh = _genArea.bounds.size.x;

        AreaLimits a = new()
        {
            x_max = x_max,
            x_min = x_min,
            z_max = z_max,
            z_min = z_min
        };

        float firstPosCoin = - ((int)(info.numOfCoins / 2) * _coinRadiusArea);
        
        // generate coin's path
        for (int i = 0; i < info.numOfCoins; i++)
        {
            GameObject _c = Instantiate(_coin, transform);
            z = firstPosCoin + (i*_coinRadiusArea);
            x = info.coinsPath.Evaluate((z/_genArea.bounds.size.z) + 0.5f) * x_max;
            
            _c.transform.localPosition = new Vector3(x, 0.2f, z);
        }
        List<Vector2> positions = SearchObstaclePositions(a, _coinCollider.radius, biggerRadius);
        for (int i = 0; i < info.numOfObstacles; i++)
        {
            int i_obs = UnityEngine.Random.Range(0, num_GO);
            x = positions[i].x;
            z = positions[i].y;
            GameObject _obs = Instantiate(_obstacles[i_obs], transform);
            _obs.transform.localPosition = new Vector3(x, 0, z);
            _obs.transform.localScale = new Vector3(1, 1, 1);

        }
    }

    /*
    void Start()
    {
        int num_GO = _obstacles.Count;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        SphereCollider _coinCollider = _coin.GetComponentInChildren<SphereCollider>();

        float _coinRadiusArea = 2*_coinCollider.radius;
        _genArea = GetComponent<BoxCollider>();
        float x_min = -_genArea.bounds.size.x / 2;
        float x_max = -x_min;
    
        float z_min = -_genArea.bounds.size.z / 2;
        float z_max = -z_min;
        float x_witdh = _genArea.bounds.size.x;
        
        for (int i = 0; i < num_obstacles; i++)
        {
            int i_obs = Random.Range(0, num_GO+1);
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
            
            if(i_obs == num_GO) //coins
            {
                for (int j = 0; j < 3 ; j++)
                {
                    z += _coinRadiusArea; 
                    
                    GameObject _c = Instantiate(_coin, transform);
                    _c.transform.localPosition = new Vector3(x, 0.2f, z);
                }
            }
            else
            {
                GameObject _obs = Instantiate(_obstacles[i_obs], transform);
                _obs.transform.localPosition = new Vector3(x, y, z);
                _obs.transform.localScale = new Vector3(1, 1, 1);
            }
            
        }
    }
    */

    
}
