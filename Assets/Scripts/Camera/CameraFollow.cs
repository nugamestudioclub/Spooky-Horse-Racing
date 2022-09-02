using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public Transform Target
    {
        get => target;
        set => target = value;
    }


    [SerializeField]
    private Camera myCamera;

    public Camera Camera => myCamera;

    [SerializeField]
    private float followStrength;




    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 newTarg = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(newTarg, transform.position, Time.deltaTime);
        }

    }


}
