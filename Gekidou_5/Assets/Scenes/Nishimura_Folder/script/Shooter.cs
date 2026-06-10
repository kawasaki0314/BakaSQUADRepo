using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField]float shootInterval = 2f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField]float randomAngle = 30f;
    [SerializeField] int BossDamege = 1;
    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= shootInterval)
        {
            timer = 0f;
            Shoot();
        }
    }
 
      
    
    void Shoot()
    {
        int bulletCount = Random.Range(2, 5);
        float angleStep = 360f/bulletCount;
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = angleStep * i + Random.Range(-randomAngle, randomAngle);
            float radian = angle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletEnemy bulletScript = bullet.GetComponent<BulletEnemy>();
            bulletScript.Initialize(direction,bulletSpeed);
        }
    }
    
}
