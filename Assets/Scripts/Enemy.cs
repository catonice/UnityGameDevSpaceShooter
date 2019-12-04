using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _xBounds = 8f;
    [SerializeField]
    private float _yBounds = 7f;
    [SerializeField]
    private float _speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-_xBounds, _yBounds), _yBounds, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Move down at 4 meters per second
        // If bottom of screen respawn at top
        // Respawn at top at a new random x position

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -_yBounds)
        {
            float randomX = Random.Range(-_xBounds, _xBounds);
            transform.position = new Vector3(randomX, _yBounds, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: ");

        if(other.CompareTag("Player"))
        {
            // Destroy us
            // Damage Player
            
            Destroy(this.gameObject);

            Debug.Log("Hit: " + other.transform.name);
        }

        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
            Debug.Log("Hit: " + other.transform.name);
        }
    }
}
