using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _yBounds = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move down at speed of 3 (adjust in inspector)
        // Destroy when leaving screen
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -_yBounds)
        {
            Destroy(this.gameObject);
        }
    }

    // OnTriggerCollision - player collect (TAGS)
    // on collected destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.TripleShotActive();
            }

            Destroy(this.gameObject);
        }
    }
}
