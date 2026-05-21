using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //方向キー入力を取得
        Vector3 v3Velocity = new Vector3(0.0f, 0.0f, 0.0f);
        v3Velocity.x = Input.GetAxis("Horizontal") * 0.015f;
        v3Velocity.y = Input.GetAxis("Vertical") * 0.015f;

        //プレイヤーの移動　
        transform.position += v3Velocity;
    }
}
