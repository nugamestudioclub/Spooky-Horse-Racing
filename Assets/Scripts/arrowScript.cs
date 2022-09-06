using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    [SerializeField]
    private GameObject gooPrefab;
    public Transform source;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), source.GetChild(1).GetComponent<Collider2D>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetDirection = rigidBody2D.velocity.normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, targetDirection);
        transform.Rotate(0, 0, 90);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Vector3 contactPt = collision.GetContact(0).point;
            GameObject goo = Instantiate(gooPrefab, contactPt, Quaternion.identity);
            goo.transform.up = collision.GetContact(0).normal;
            goo.GetComponent<Animator>().Play("gooDeploy");
            goo.GetComponent<GooScript>().source = source;
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<RollPhysics>().Freeze(1.5f);
            source.root.GetComponent<RacePlayer>().HitCount++;
            Destroy(gameObject);
        }
    }
}
