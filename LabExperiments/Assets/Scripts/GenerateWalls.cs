using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWalls : MonoBehaviour
{
    [SerializeField] private GameObject _elementWall;
    [SerializeField] private List<Transform> _walls;
    
    /*
    offsetAngle is an angle, 
    offsetScale is a percentage, 
    offsetPos is a position and it depends on bounding box
    */
    private float offsetAngle = 30, offsetScale = 0.1f, offsetPos; 
    private float radius = 6.0f;

    // Start is called before the first frame update

    void Start()
    {
        CapsuleCollider _elementCollider = _elementWall.GetComponent<CapsuleCollider>();
        float diamiter = 2 * _elementCollider.radius;
        if(_walls != null && _walls.Count > 0) {
            foreach(Transform wall in _walls){
                BoxCollider _wallCollider = wall.gameObject.GetComponent<BoxCollider>();
                
                int _numElement = (int)(_wallCollider.size.z / diamiter);
                offsetPos = (_wallCollider.size.x - diamiter) / 2; // a range to randomize a little the position inside the wall's BoxCollision 
                for (int i = 0; i < _numElement; i++)
                {
                    GenerateRandWall(wall, new Vector3(0, 0, (-(_wallCollider.size.z-diamiter)/2)+(i*diamiter)));
                }
            }
               
        }
        else
        {
            Vector3 center = transform.position;
            int _numElement = (int)(2*Mathf.PI*radius / diamiter);
            float step = (float)(2 *Math.PI / _numElement);
            for (int i = 0; i < _numElement; i++)
            {   
                offsetPos = 0.125f;
                float x = center.x + (Mathf.Cos(i*step) * radius);
                float z = center.z + (Mathf.Sin(i*step) * radius);

                GenerateRandWall(transform, new Vector3(x,0,z));
            }
        }
        
    }

    private void GenerateRandWall(Transform parent, Vector3 localPosition)
    {
        float rotY = UnityEngine.Random.Range(-offsetAngle, offsetAngle);
        float sclY = UnityEngine.Random.Range(-offsetScale, offsetScale);
        float posX = UnityEngine.Random.Range(-offsetPos, offsetPos);
        //float posY = UnityEngine.Random.Range(-offsetPos, offsetPos);
        float posZ = UnityEngine.Random.Range(-offsetPos, offsetPos);
        //Debug.Log("rotYR "+rotYR+" posXR " + posXR + " sclZR " + sclZR);

        GameObject _elWall = Instantiate(_elementWall, parent);
        _elWall.transform.localPosition = localPosition + new Vector3(posX, 0, posZ);
        _elWall.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        _elWall.transform.localScale += new Vector3(0, sclY * _elementWall.transform.localScale.y, 0); 
    }
}
