using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 10f; //弾の移動速度
    private Vector2 direction; //弾の移動方向


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime; //弾の移動
    }
}
