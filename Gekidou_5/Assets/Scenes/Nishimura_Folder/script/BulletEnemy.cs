using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletEnemy : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    private Vector2 direction;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] int BossDamege = 1;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir;
        moveSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * moveSpeed;

        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            levelupplayer player = other.GetComponent<levelupplayer>();
            player.hp -= BossDamege;
            Destroy(gameObject);
            Debug.Log("当たった!");

        }

    }
}

