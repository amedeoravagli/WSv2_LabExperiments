using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWalls : MonoBehaviour
{
    [SerializeField] private GameObject _elementWall;
    [SerializeField] private Transform _rWall;
    [SerializeField] private Transform _lWall;
    /*
    offsetAngle is an angle, 
    offsetScale is a percentage, 
    offsetPos is a position and it depends on bounding box
    */
    private float offsetAngle = 30, offsetScale = 0.1f, offsetPos; 
    

    // Start is called before the first frame update

    void Start()
    {
        BoxCollider _wallCollider = _rWall.gameObject.GetComponent<BoxCollider>();
        CapsuleCollider _elementCollider = _elementWall.GetComponent<CapsuleCollider>();
        float diamiter = 2 * _elementCollider.radius;
        int _numElement = (int)(_wallCollider.size.z / diamiter);
        offsetPos = (_wallCollider.size.x - diamiter) / 2;
        for (int i = 0; i < _numElement; i++)
        {
            float rotYR = Random.Range(-offsetAngle, offsetAngle);
            float rotYL = Random.Range(-offsetAngle, offsetAngle);
            float posXR = Random.Range(-offsetPos, offsetPos);
            float posXL = Random.Range(-offsetPos, offsetPos);
            float sclYR = Random.Range(-offsetScale, offsetScale);
            float sclYL = Random.Range(-offsetScale, offsetScale);
            //Debug.Log("rotYR "+rotYR+" posXR " + posXR + " sclZR " + sclZR);

            GameObject _elWallL = Instantiate(_elementWall, _lWall);
            _elWallL.transform.localPosition = new Vector3(posXL, 0, (-(_wallCollider.size.z-diamiter)/2)+(i*diamiter));
            _elWallL.transform.localRotation = Quaternion.Euler(0, rotYL, 0);
            _elWallL.transform.localScale += new Vector3(0, sclYL * _elementWall.transform.localScale.y, 0);  
            
            GameObject _elWallR = Instantiate(_elementWall, _rWall);
            _elWallR.transform.localPosition = new Vector3(posXR, 0, (-(_wallCollider.size.z-diamiter)/2)+(i*diamiter));
            _elWallR.transform.localRotation = Quaternion.Euler(0, rotYR, 0);
            _elWallR.transform.localScale += new Vector3(0, sclYR * _elementWall.transform.localScale.y, 0);
            

        }
    }
}
