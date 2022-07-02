using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroyAudio : MonoBehaviour
{

    public static DoNotDestroyAudio instance;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
