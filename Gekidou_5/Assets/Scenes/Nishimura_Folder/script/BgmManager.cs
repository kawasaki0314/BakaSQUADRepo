using Microsoft.Win32.SafeHandles;
using UnityEditor;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    float timer = 0f;
    [SerializeField]AudioSource tureaudio;
    Transform playerTransform;
    bool bgmDestroy = false;
    //public GameObject BossPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10f && !bgmDestroy)
        {
            Destroy(gameObject);
            bgmDestroy = true;

        }
    }
    //void BgmDestroy(GameObject gameObject)
    //{
    //    timer += Time.deltaTime;

    //    if (timer < 1f && !bgmDestroy)
    //    {
    //        Destroy(gameObject);

    //    }
    //}
}
