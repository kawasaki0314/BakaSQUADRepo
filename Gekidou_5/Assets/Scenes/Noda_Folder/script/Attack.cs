using UnityEngine;

public class Attack : MonoBehaviour
{

    private Animator anim; //インスペクターでAnimatorを紐づけるための変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //自身のオブジェクトについているAnimatorコンポーネントを取得
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("attack");
        }
    }
}
