using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 2;
    [SerializeField] float lifeTime = 3f;
    Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector2(-speed, 0);
        StartCoroutine(DestroyAfterDelay(lifeTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && collision.gameObject.tag != "Enemy"))
        {
            Debug.Log("Projectile destroyed by " + collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
