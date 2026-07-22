using UnityEngine;

public class hp : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] int maxHp = 1;
    public int currentHp = 1;
    

    private chicken chicken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        chicken = GetComponent<chicken>();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            chicken.Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
