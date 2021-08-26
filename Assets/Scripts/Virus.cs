using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public Rigidbody2D Rbody;

    private bool _active;

    private void Start()
    {
        _active = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) { _active = true; }
        if (Input.GetKeyDown(KeyCode.Y)) { _active = false; }

        if (_active)
        {
            Rbody.AddForce(new Vector2
                (Random.Range(-1f,1f),
                Random.Range(-1f, 1f))
                * 40f);
        }

    }
}
