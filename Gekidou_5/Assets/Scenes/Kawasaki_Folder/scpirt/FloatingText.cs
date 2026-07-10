using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // テキストが上に移送する速度
    public float moveSpeed = 1.5f;

    // テキストの表示時間
    public float lifeTime = 1f;

    // このオブジェクトについているTextMeshProUGUI
    // を取得するための変数
    private TextMeshPro text;
    private float maxLifeTime;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();

        // 初期寿命の保存
        maxLifeTime = lifeTime;
    }
    /// <summary>
    /// 外部から表示する文字を設定する
    /// </summary>
    /// <param name="message">表示したい文字列</param>
    public void SetText(string message)
    {
        text.text = message;
    }


    // Update is called once per frame
    private void Update()
    {
        // 毎フレーム少しずつ上方向へ移動させる
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 残り表示時間を減らす
        lifeTime -= Time.deltaTime;

        // 残り時間に応じて透明度を変更
        Color color = text.color;
        color.a = lifeTime / maxLifeTime;
        text.color = color;

        // 表示時間が0以下になったら自分自身を削除
        if(lifeTime<=0)
        {
            Destroy(gameObject);
        }
    }
}
