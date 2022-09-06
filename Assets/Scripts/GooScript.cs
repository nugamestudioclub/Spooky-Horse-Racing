using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooScript : MonoBehaviour
{
    [SerializeField]
    private float longevity = 2;
    private Animator animator;
    public Transform source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Decay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<RacePlayerMovement>().Freeze();
            if (!ReferenceEquals(source, collision.transform.root))
            {
                source.GetComponent<RacePlayer>().HitCount++;
                Debug.Log("indirect hit");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<RacePlayerMovement>().UnFreeze();
        }
    }

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(longevity + .2f);

        animator.Play("gooDecay");

        yield return new WaitForSeconds(1.267f);

        Destroy(gameObject);
    }
}
