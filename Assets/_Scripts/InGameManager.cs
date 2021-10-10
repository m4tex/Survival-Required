using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public GameObject _player;
    public static GameObject player;

    private void Awake()
    {
        player = _player;
    }
}
