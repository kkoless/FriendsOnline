using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using UnityEngine.UI;


public class NetWork : MonoBehaviour
{
    private static string Token;
    public CanvasGroup mainPanelCG;
    public CanvasGroup secondPanelCG;
    public CanvasGroup buttonShowFriends;

    // Метод авторизации пользователя
    public void Authorization(string login, string pass)
    {
        AuthorizeAsync(login, pass).Wait();
    }

    // Асинхронная авторизация
    public Task AuthorizeAsync(string login, string pass)
    {
        var api = new VkApi();

        var rTask = new Task(() => api.Authorize(new ApiAuthParams
        {
            ApplicationId = 7658547,
            Login = login,
            Password = pass,
            Settings = Settings.All,

        }));        
        rTask.Start();
        rTask.Wait();
        Root.Instance.UI.setActive(buttonShowFriends);
        Root.Instance.UI.setUnActive(mainPanelCG);
        Root.Instance.UI.setActive(secondPanelCG);
        
        Token = api.Token;
        return rTask;
    }

    // Метод, который возвращает токен пользователя
    public static string getToken()
    {
        return Token;
    }
}

