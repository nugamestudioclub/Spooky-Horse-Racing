using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private float bowPower = 5;
    private float angle;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        float a = mousePos.x - transform.position.x;
        float b = mousePos.y - transform.position.y;
        angle = Mathf.Atan2(b, a) * 180 / Mathf.PI;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0)) chargeBow();

        if (Input.GetMouseButtonUp(0)) fireBow();
    }

    private void chargeBow()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.Play("bowCharge");
        }
    }

    private void fireBow()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("bowCharge"))
        {
            float charge = Mathf.Round(animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 10 / 2) + 1;
            if (charge > 6) charge = 6;
            GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.right * 2, transform.rotation);
            Rigidbody2D rigidbody2D = arrow.GetComponent<Rigidbody2D>();
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

            rigidbody2D.AddForce(dir * charge * bowPower, ForceMode2D.Impulse);

            animator.Play("fireReload");
        }
    }
}
