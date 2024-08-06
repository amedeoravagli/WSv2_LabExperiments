using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMotion : MonoBehaviour
{
    
    public float _speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 0, (Time.deltaTime * _speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
