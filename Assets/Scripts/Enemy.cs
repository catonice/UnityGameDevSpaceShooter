﻿using System.Collections;
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

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        // Randomized spawn now controlled by Spawn Manager
        // transform.position = new Vector3(Random.Range(-_xBounds, _yBounds), _yBounds, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        // GetComponent is an expensive call so we cache the player object when an enemy is created
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }

            Destroy(this.gameObject);

            Debug.Log("Hit: " + other.transform.name);
        }

        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player)
            {
                _player.AddScore(10);
            }

            Destroy(this.gameObject);
        }
    }
}
