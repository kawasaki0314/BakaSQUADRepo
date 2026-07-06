using System;
using UnityEngine;

public class Enemyanims : MonoBehaviour
{
    public enum AnimState { walk, shot};
    public AnimState animState = AnimState.walk;
    //コンポーネントを取得するためのもの
    SpriteRenderer spriteRenderer;
    //アニメーション画像の配列宣言
    [SerializeField] Sprite[] walkAnim;
    [SerializeField] Sprite[] shotAnim;
    //1コマが表示されるFrame
    int animFrame = 10;
    //切り替え用のカウンター
    int walkFrameTimer = 0;
    int shotFrameTimer = 0;
    //アニメーションの最大数
    int walkAnimMax = 0;
    int shotAnimMax = 0;
    //現在のコマ数
    int walkAnimIdx; 
    int shotAnimIdx;

    //プレイヤーのtransform
    Transform playerTr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントでGetできる
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animMaxで配列の長さを代入
        shotAnimMax = shotAnim.Length;
        walkAnimMax = walkAnim.Length;

        //プレイヤーを探して取得
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        if (playerobj != null) playerTr = playerobj.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //向きの反転処理
        if(playerTr != null)
        {
            float deltaX = playerTr.position.x - transform.position.x;
            if(deltaX > 0.001f)
            {
                spriteRenderer.flipX = false;
            }
            else if(deltaX < -0.0001f)
            {
                spriteRenderer.flipX = true;
            }
        }
        
        switch(animState)
        {
            case AnimState.walk:
                walkFrameTimer++;
                if(walkFrameTimer >= animFrame)
                {
                    walkFrameTimer = 0;
                    walkAnimIdx++;
                    //アニメーションが最大数に達したらコマ数を0に戻す処理
                    if (walkAnimIdx >= walkAnimMax)
                    {
                        walkAnimIdx = 0;
                    }
                }
                //見た目をコマ数に応じて変更する処理
                spriteRenderer.sprite = walkAnim[walkAnimIdx];
                // 変数の初期化
                shotFrameTimer = 0;
                shotAnimIdx = 0;
                break;
                
               
            case AnimState.shot:
                shotFrameTimer++;
                if (shotFrameTimer >= animFrame)
                {
                    shotFrameTimer = 0;
                    shotAnimIdx++;
                    //アニメーションが最大数に達したらコマ数を0に戻す処理
                    if (shotAnimIdx >= shotAnimMax)
                    {
                        animState = AnimState.walk;
                        shotAnimIdx = 0;
                    }
                }
                //見た目をコマ数に応じて変更する処理
                spriteRenderer.sprite = shotAnim[shotAnimIdx];
                // 変数の初期化
                walkFrameTimer = 0;
                walkAnimIdx = 0;
                break;
        }
    }
}
