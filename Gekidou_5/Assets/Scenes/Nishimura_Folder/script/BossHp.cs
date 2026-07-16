using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHp : MonoBehaviour
{
   public int bossHp = 1500;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        bossHp -= damage;

        if(bossHp <= 0 )
        {
            SceneManager.LoadScene("Clear");
            Destroy(gameObject);
        }

    }

}
