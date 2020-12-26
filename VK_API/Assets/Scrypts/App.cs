using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[System.Serializable]
public class App : MonoBehaviour
{

    // Метод, с помощью которого можно обращаться к различным методам VK API
    public static string MethodAPI(string MethodName, string Parametrs)
    {
        string Token = NetWork.getToken();
        var req = HttpWebRequest.Create(string.Format("https://api.vk.com/method/{0}?{1}&v=5.52&access_token={2}", MethodName, Parametrs, Token));
        var resp = req.GetResponse();
        using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8)) return stream.ReadToEnd();
    }

    // Метод, который возвращает список ID пользователей, находящихся Online
    public List<int> getOnlineID()
    {
        string res = MethodAPI("friends.getOnline",null);

        UsersOnline usersOnline = JsonUtility.FromJson<UsersOnline>(res);
  
        return usersOnline.response;
    }

    // Класс, который нужен для парса(JSON) листа ID пользователей, находящихся Online
    class UsersOnline
    {
        public List<int> response;
    }


    // Метод, который возвращает данные о пользователе (Имя, Фамилия) по ID
    public string[] getOnlineInfoUsers(List<int> ids)
    {
        string resArr = "";
        string idsStr = String.Join(",",ids);
        
        resArr = MethodAPI("users.get", "user_ids=" + idsStr + "&");

        Info[] infoUsers = new Info[] { };
        infoUsers = JsonHelper.FromJson<Info>(resArr);

        string[] res = new string[infoUsers.Length];
        for (int i = 0; i < infoUsers.Length; i++)
        {
            res[i] = infoUsers[i].first_name + " " + infoUsers[i].last_name;
        }

        return res;
    }

    // Метод, который возвращает ссылки на аватарки пользователй
    public string[] getOnlinePhotosURL(List<int> ids)
    {
        string resArr = "";
        string idsStr = String.Join(",", ids);

        resArr = MethodAPI("users.get", "user_ids=" + idsStr + "&" + "fields=photo_100&");

        Info[] infoUsers = new Info[] { };
        infoUsers = JsonHelper.FromJson<Info>(resArr);

        string[] res = new string[infoUsers.Length];
        for (int i = 0; i < infoUsers.Length; i++)
        {
            res[i] = infoUsers[i].photo_100;
        }

        return res;
    }

    // Класс-хелпер для работы с JSON массивами
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.response;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.response = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.response = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] response;
        }
    }

    // Класс, который нужен для парса(JSON) данных пользователей, находящихся Online
    [Serializable]
    class Info
    {
        public string first_name;
        public int id;
        public string last_name;
        public bool can_access_closed;
        public bool is_closed;
        public string photo_100;
    }

}

