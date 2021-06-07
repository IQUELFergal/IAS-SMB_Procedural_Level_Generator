using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Coin : MonoBehaviour
{
    [SerializeField] GameEvent onCoinPickUpEvent;
    [SerializeField] ParticleSystem coinEffect;
    private SpriteRenderer sr;
    private Tilemap tilemap;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tilemap = Object.FindObjectOfType(typeof(Tilemap)) as Tilemap;
        tilemap.SetTile(tilemap.WorldToCell(transform.position), null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onCoinPickUpEvent?.Invoke();
            sr.enabled = false;
            coinEffect.gameObject.SetActive(true);
            coinEffect.Stop();
            coinEffect.Play();
            StartCoroutine(DestroySelfAfterDuration(coinEffect.main.startLifetime.constant));
        }
    }

    IEnumerator DestroySelfAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        coinEffect.Stop();
        //coinEffect.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
