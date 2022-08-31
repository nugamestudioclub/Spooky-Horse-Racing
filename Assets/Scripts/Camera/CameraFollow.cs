using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float followStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newTarg = new Vector3(target.transform.position.x,target.transform.position.y + 5,transform.position.z);
        transform.position = Vector3.Lerp(newTarg, transform.position, Time.deltaTime);
    }
}
