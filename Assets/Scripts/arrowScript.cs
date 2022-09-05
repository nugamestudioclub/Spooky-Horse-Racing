using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = rigidBody2D.velocity.normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, targetDirection);
        transform.Rotate(0, 0, 90);
    }

   

}
