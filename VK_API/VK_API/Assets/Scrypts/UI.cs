using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void setActive(CanvasGroup a) {
        a.alpha = 1;
        a.interactable = true;
    }
    public void setUnActive(CanvasGroup a)
    {
        a.alpha = 0;
        a.interactable = false;
    }
    public void sayHello() {
        print("Привет!");
    }
}
