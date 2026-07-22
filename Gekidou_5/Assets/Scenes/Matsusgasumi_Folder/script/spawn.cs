using UnityEngine;

public class spawn : MonoBehaviour
{
    [Header("隠し敵の設定")]
    [SerializeField] GameObject chickenPrefab; // インスペクターでchickenをセット
    [Range(0f, 100f)]
    [SerializeField] float chickenSpawnRate = 20.0f; // 5回に1回ぐらい
    public static spawn Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        float spawnChamce = Random.Range(0.0f, 100.0f);
        if (spawnChamce <= 5.0f)
        {
            Debug.Log("レアアイテムがスポーンしました！");
        }
    }

    void Update()
    {

    }

    public void OnEnemyDefeated(bool isRare, Vector3 deadPosition)
    {
        Debug.Log("敵が倒された位置: " + deadPosition);

        float chickenChance = Random.Range(0.0f, 100.0f);
        if (chickenChance <= chickenSpawnRate)
        {
            Instantiate(chickenPrefab, deadPosition, Quaternion.identity);
            Debug.Log("隠し敵(chicken)が出現しました！");
        }
        else
        {
            
        }
    }
}