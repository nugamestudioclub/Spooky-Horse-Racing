using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RacePlayer>().ControlEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
