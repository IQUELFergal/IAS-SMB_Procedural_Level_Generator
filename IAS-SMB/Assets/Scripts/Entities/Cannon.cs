using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] float cooldown = 2;
    [SerializeField] GameObject projectile = null;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(Shoot(cooldown));
    }

    IEnumerator Shoot(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Debug.Log("Shoot");
        Instantiate(projectile, transform.position, Quaternion.identity, transform);
        StartCoroutine(Shoot(cooldown));
    }
}
