using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noodle : MonoBehaviour
{
    public NoodleTypes.types noodleType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EndBorder"))
        {
            Destroy(gameObject);
        }
    }
}
