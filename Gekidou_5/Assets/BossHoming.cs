using System;
using UnityEngine;

public class BOoss : MonoBehaviour
{
    Transform playerTr;//プレイヤーのtrasform
    [SerializeField] float speed = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTr.position) < 0.1f)
            return;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTr.position.x, playerTr.position.y), speed * Time.deltaTime);
    }
}
