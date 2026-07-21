using UnityEngine;

public class spawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float spawnChamce = Random.Range(0.0f, 100.0f);
        if(spawnChamce <= 5.0f)
        {
            Debug.Log("レアアイテムがスポーンしました！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
