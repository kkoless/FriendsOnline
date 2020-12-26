using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    public static Root Instance {
        get{
            if (_instance == null) {
                _instance = FindObjectOfType<Root>();
            }
            return _instance;
        }
    }
    private static Root _instance;

    public NetWork NetWork;

    public App App;

    public Data Data;

    public UI UI;
}
