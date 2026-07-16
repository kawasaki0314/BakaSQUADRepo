using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletEnemy : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    private Vector2 direction;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] int BossDamege = 10;
    Rigidbody2D rb;
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
     void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(collision.CompareTag("Player"))
        {
            levelupplayer player = collision.GetComponent<levelupplayer>();
            player.hp -= BossDamege;
            Destroy(gameObject);
            Debug.Log("当たった!");

        }

    }
}

