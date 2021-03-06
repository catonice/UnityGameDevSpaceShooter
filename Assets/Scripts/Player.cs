﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Under score for private variables
    // SerializeField attribute allows us to serialize data and read and overwrite it in the inspector
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _UIManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Take current pos and set start position = new pos(0,0,0)
        transform.position = new Vector3(0, 0, 0);

        // Find game object
        // Caching objects
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        if (!_spawnManager)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (!_UIManager)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
    }

    // Update is called once per frame (typically 60fps)
    void Update()
    {
        CalculatePlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        // Time.time is how long the game has been running

        _canFire = Time.time + _fireRate;

        // Debug.Log("Space key pressed");
        // Euler angle -> Quarternarion?
        // Quaternion.identity is default rotation
        // Vector3 offset = new Vector3(0, 0.8f, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

    }

    void CalculatePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 for left arrow (a), 0 for no input, 1 for right arrow (d)
        float verticalInput = Input.GetAxis("Vertical"); // -1 for left arrow (a), 0 for no input, 1 for right arrow (d)

        // Unity has predefined Vector - Vector3.right === new Vector(1 unit(meter)) ,0,0)
        // new Vector3(1,0,0) (right) opposite is left
        // Convert 1 meter per frame to 1 meter per second - frame rate to real time - deltaTime == 1 sec - time from 1 last frame to the current frame
        // Real time
        // For e.g. 1 * 0 * 3.5f * real time = 0

        // Old line: transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // Old line: transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // More optimal now because we are newing only 1 Vector3 instead of 2

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        // if player positionon the y greater than 0 
        // set player position back to 0
        // if player x is greater than 9 set player x to 9

        float yBounds = 3.8f;
        float xBounds = 11.3f;

        /* Replaced by the clamp
         *if (transform.position.y >= 0)
         {
             transform.position = new Vector3(transform.position.x, 0, 0);
         }
         else if (transform.position.y <= -yBounds)
         {
             transform.position = new Vector3(transform.position.x, -yBounds, 0);
         }
        */

        // Clamp function (sets value to inbetween min and max values) (value, min value, max value)
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -yBounds, yBounds), 0);

        if (transform.position.x > xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, 0);
        }
        else if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, 0);
        }
    }

    public void Damage() 
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives -= 1;

        if (_lives < 1) 
        {
            if (_spawnManager) 
            {
                _spawnManager.OnPlayerDeath();
            }

            Destroy(this.gameObject);
        }
    }

    // TriShot power up
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    // Speed power up
    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed = 10f;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedActive = false;
        _speed = 5f;
    }

    // Shield power up
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points) {
        _score += points;
        _UIManager.UpdateScoreText(_score);
    }
}
