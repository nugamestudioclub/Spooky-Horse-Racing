using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollPhysics : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 10f;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.AddTorque(-Input.GetAxis("Horizontal") * Time.deltaTime * speed,ForceMode2D.Impulse);
    }
}
