﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable of 8
    [SerializeField]
    private float _speed = 8f;

    // Update is called once per frame
    void Update()
    {
        // translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        float yBounds = 8f;

        if (transform.position.y >= yBounds)
        {
            // this <- is unneccessary just for readability
            Destroy(this.gameObject);
            // Destroy(this.gameObject, 5f); <--- destroy in 5 seconds
        }
    }
}
