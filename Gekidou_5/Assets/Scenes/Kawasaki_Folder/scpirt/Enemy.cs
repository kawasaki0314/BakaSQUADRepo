using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 dir, float power)
    {
        rb.AddForce(dir * power, ForceMode2D.Impulse);
        StartCoroutine(StopAfterTime());
    }

    IEnumerator StopAfterTime()
    {
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;
    }
}
