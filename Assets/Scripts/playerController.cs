using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Transform rightArmBone;
    private Transform leftBicepBone;
    private Transform leftArmBone;
    private Transform rightArm;
    private Transform bow;
    private float angle;
    private bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        rightArmBone = transform.Find("rightArmBone");
        leftBicepBone = transform.Find("leftBicepBone");
        leftArmBone = leftBicepBone.Find("leftArmBone");
        rightArm = transform.Find("rightArm");
        bow = rightArm.Find("bow");
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, rightArmBone.position.z - Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        float armX = mousePos.x - rightArmBone.position.x;
        float armY = mousePos.y - rightArmBone.position.y;
        angle = Mathf.Atan2(armY, armX) * 180 / Mathf.PI;

        rightArmBone.rotation = Quaternion.Euler(0, 0, angle); 
        leftBicepBone.rotation = Quaternion.Euler(0, 0, angle);
        bow.rotation = Quaternion.Euler(0, 0, angle);

        if ((angle > 90 && angle < 270) || (angle < -90 && angle > -270))
        {
            transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }

        Transform thighBone = transform.Find("torsoBone").Find("thighBone");
        thighBone.rotation = Quaternion.Euler(0, 0, thighBone.rotation.eulerAngles.z);

        if (!reloading)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(PullBack());
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            reloading = true;
            leftArmBone.localPosition = new Vector3(0.5850446f, 1.882562e-07f, 0);
            StartCoroutine(WaitToReload());
        }

        // Following code is for demo purposes
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(50 * Time.deltaTime, 0, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-50 * Time.deltaTime, 0, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.S) && transform.position.y > -9)
        {
            transform.Translate(new Vector3(0, -50 * Time.deltaTime, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 50 * Time.deltaTime, 0), Space.World);
        }
        // end demo code
    }

    private IEnumerator WaitToReload()
    {
        while (bow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fireReload"))
        {
            yield return null;
        }

        reloading = false;
    }


    private IEnumerator PullBack()
    {
        yield return new WaitForSeconds(0.2f);

        while (leftArmBone.transform.localPosition.x > 0.2)
        {
            leftArmBone.Translate(-.19f, 0, 0);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
