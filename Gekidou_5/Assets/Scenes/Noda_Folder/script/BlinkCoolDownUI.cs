using UnityEngine;
using UnityEngine.UI; //UI操作に必要

public class BlinkCoolDownUI : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider; //エディタからSliderをアタッチ 
    [SerializeField] private PlayerMove playerMove; //エディタからPlayerMoveをアタッチ

    void Update()
    {
        if(playerMove == null || cooldownSlider == null) 
        return;

        //クールタイムの残り時間を計算
        //残り時間が0に近いほど、ゲージが溜まっている(または残っている)
        float cooldownMax = playerMove.blinkCooldown;
        float currentCooldown = playerMove.GetCoolDownTimer();

        if(cooldownMax > 0)
        {
            //使うとゲージがなくなり、時間経過で溜まり満タンになると再使用可能
            cooldownSlider.value = (cooldownMax - currentCooldown) / cooldownMax;
        }
    }
}
