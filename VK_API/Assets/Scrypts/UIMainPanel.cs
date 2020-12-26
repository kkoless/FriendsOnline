using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPanel : MonoBehaviour
{
    public InputField input1;
    public InputField input2;
    public CanvasGroup mainPanelCG; 
    public UISecondPanel UISecondPanel;
    

    // Метод, который "активирует" вторую панель в приложении
    public void secondPanelActive()
    {
        Root.Instance.UI.setUnActive(mainPanelCG);
        Root.Instance.UI.setActive(UISecondPanel.secondPanelCG);
    }

    // Метод авторизации пользователя, который выполняется при нажатии кнопку
    public void AuthorizationUser()
    {
        Root.Instance.NetWork.Authorization(input1.text, input2.text);
    }

}
