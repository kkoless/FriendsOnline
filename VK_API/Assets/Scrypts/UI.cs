using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI : MonoBehaviour
{
    public void setActive(CanvasGroup a) {
        DOTween.To(()=> a.alpha, x=> a.alpha = x, 1, 2f).SetEase(Ease.InQuad).SetUpdate(true);
        //a.alpha = 1;
        a.interactable = true;
    }
    public void setUnActive(CanvasGroup a)
    {
        DOTween.To(() => a.alpha, x => a.alpha = x, 0, 1f).SetEase(Ease.InQuad).SetUpdate(true);
        //a.alpha = 0;
        a.interactable = false;
    }
}
