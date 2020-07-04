using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noodle : MonoBehaviour
{
    public float speed = 3f;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EndBorder"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bowl"))
        {
            Debug.Log("Counted!");
            Destroy(gameObject);
        }
    }
}
