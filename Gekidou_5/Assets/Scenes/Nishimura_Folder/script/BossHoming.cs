using System;
using UnityEngine;

public class BOoss : MonoBehaviour
{
    Transform playerTr;//プレイヤーのtrasform
    [SerializeField] float speed = 2;
    [SerializeField] private float rotationSpeed = 90f;
    Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        //BossHp = BossHp * currentlevel;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTr.position) < 2.0f)
            return;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTr.position.x, playerTr.position.y), speed * Time.deltaTime);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        rb.linearVelocity = Vector2.zero;

    }
}
