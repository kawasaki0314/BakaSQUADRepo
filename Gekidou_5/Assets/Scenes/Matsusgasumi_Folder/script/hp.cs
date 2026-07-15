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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
