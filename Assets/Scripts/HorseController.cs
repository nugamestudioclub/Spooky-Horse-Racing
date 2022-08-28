using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D target;

    private Vector2 prevPos;
    

    // Start is called before the first frame update
    void Start()
    {
        prevPos = target.transform.position;   
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = target.transform.position;
        
    }
    private void FixedUpdate()
    {

        Vector2 delta = (Vector2)target.transform.position - prevPos;
        float angle = (Mathf.Atan2(delta.y, delta.x)/(Mathf.PI*2))*360;
        // print(angle);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            angle);
        prevPos = target.transform.position;
    }
}
