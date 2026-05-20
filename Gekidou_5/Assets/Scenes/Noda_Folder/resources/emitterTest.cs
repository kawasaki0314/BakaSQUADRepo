using UnityEngine;
using Effekseer;

public class emitterTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //エフェクトを取得
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>("Laser01");
        //transformの位置でエフェクトを再生
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, transform.position);
        //transformの回転を設定
        handle.SetRotation(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
