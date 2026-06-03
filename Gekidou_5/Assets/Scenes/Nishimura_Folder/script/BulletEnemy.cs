using UnityEngine;
using UnityEngine.Rendering;

public class BulletEnemy : MonoBehaviour
{

    [SerializeField]float moveSpeed = 5f;
    private Vector2 direction;
    [SerializeField] float lifeTime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
 
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir;
        moveSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}
