using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float force;

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(force, 0));
        Destroy(gameObject, 2f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Target")
        {
            GameObject.Find("Bottle Sound").GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
            Destroy(gameObject, 0);
        }
        if (other.name == "Title")
        {
            GameObject.Find("Bottle Sound").GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    }
}
