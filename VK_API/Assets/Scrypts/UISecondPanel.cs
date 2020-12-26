using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UISecondPanel : MonoBehaviour
{
    public CanvasGroup buttonCG2;
    public CanvasGroup secondPanelCG;
    public CanvasGroup buttonUpdateCG;
    public CanvasGroup buttonExitCG;
    public GameObject gameObject;
    public Transform transform;
    public RectTransform contentRectTransform;
    public Sprite targetSprite;
    public SpriteRenderer spriteRenderer;
    public Scrollbar scrollbar;
    public CanvasGroup imageDetectiveCG;
    public GameObject[] blocks;
    public CanvasGroup contentCG;

    // Метод, который показывает блок с пользователями Online
    public void showFriendsOnClick()
    {
        Root.Instance.UI.setUnActive(buttonCG2);
        Root.Instance.UI.setUnActive(imageDetectiveCG);
        Root.Instance.UI.setActive(buttonExitCG);
        Root.Instance.UI.setActive(buttonUpdateCG);
        spawnOBJs(Root.Instance.App.getOnlineInfoUsers(Root.Instance.App.getOnlineID()), Root.Instance.App.getOnlinePhotosURL(Root.Instance.App.getOnlineID()));
        //Root.Instance.UI.setActive(contentCG);
    }

    // Метод, который генерирует "блоки" пользователей
    public void spawnOBJs(string[] names, string [] urls)
    {
        
        blocks = new GameObject[names.Length];

        double l = names.Length;
        if (l > 16 && l <= 20) {
            contentRectTransform.sizeDelta = new Vector2(783, 640 + 60);
        }

        if (l > 20)
        {
            double dif = (l / 4.0) - 4.0;               
            int res = Convert.ToInt16(Math.Ceiling(dif));  
            contentRectTransform.sizeDelta = new Vector2(783, 640 + res * 126 + 60);
        }

        for (int i = 0; i < names.Length; i++)
        {
            SetPhotosText(names[i],blocks[i],urls[i]);                     
        }
        Root.Instance.UI.setActive(contentCG);
    }

    // Метод, который устанавливает значения для полей блока (Текст, Картинка)
    public void SetPhotosText(string id, GameObject game, string url)
    {
        StartCoroutine(GetTextureRequest(url, (response) => {
            targetSprite = response;
            spriteRenderer.sprite = targetSprite;
            game = gameObject;
            game.GetComponentInChildren<Text>().text = id;
            game.GetComponentInChildren<Image>().sprite = spriteRenderer.sprite;
            
            Instantiate(game, transform, false);
        }));
        
    }

    // Метод, который обновляет информацию о пользователях
    public void updateFriendsOnline() {
        contentCG.alpha = 0;
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        contentRectTransform.sizeDelta = new Vector2(783, 640);
        scrollbar.value = 1;
        spawnOBJs(Root.Instance.App.getOnlineInfoUsers(Root.Instance.App.getOnlineID()), Root.Instance.App.getOnlinePhotosURL(Root.Instance.App.getOnlineID())); ;
        
    }

    // Метод, который отвечает за выход из приложения
    public void exit() {
        Application.Quit();
    }

    // Метод, который скачивает картинки по ссылке
    IEnumerator GetTextureRequest(string url, System.Action<Sprite> callback)
    {
        using (var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    var rect = new Rect(0, 0, 50f, 50f);
                    var sprite = Sprite.Create(texture, rect, new Vector2(50f, 50f));
                    callback(sprite);
                }
            }
        }
    }

}
