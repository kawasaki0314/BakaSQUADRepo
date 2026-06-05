using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemprefab;
    public GameObject itemprefab1;
    public GameObject itemprefab2;

    [SerializeField] float xLimit = 8.0f;//x座標の制限
    [SerializeField] float yLimit = 4.0f;//y座標の制限

    float span = 1.0f;//生成間隔
    [SerializeField] float spanMin = 3.0f;//生成間隔の最小値
    [SerializeField] float spanMax = 6.0f;//生成間隔の最大値
    float delta = 0f;//経過時間
    float SpawnerIndex = 0;

    //[SerializeField]はUnityエディタ上で変数を編集できるようにするための属性です。これにより、spanMinやspanMaxなどの値をエディタ上で簡単に調整できます。

    void Start()
    {
        span = Random.Range(spanMin, spanMax);//spanの値を3から6の中からランダムに生成するコード

    }
    void Update()
    {

        this.delta += Time.deltaTime;//Delta値を秒ごとに増やすコード
        if (this.delta >= this.span)//spanの値を超えるごとに実行されるコード
        {
            
           
            this.delta = 0f;//上のコードが実行されたときDeltaの値を0にするコード
            span = Random.Range(spanMin, spanMax);//spanの値をspanMinからspanMaxの中からランダムに生成するコード
            GameObject obj = Instantiate(itemprefab);//itemprefabを生成してobjに代入するコード
            float px = Random.Range(-xLimit, xLimit);//指定したx座標の中からランダムに値を生成するコード
            float py = Random.Range(-yLimit, yLimit);//指定したy座標の中からランダムに値を生成するコード

            obj.transform.position = new Vector3(px, py, 0);//上で生成したobjの位置をpxとpyの値にするコード
        }
    }
}
