using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMainPanel : MonoBehaviour
{
    public Button button;
    public InputField input1;
    public InputField input2;
    public CanvasGroup buttonCG;
    public CanvasGroup input1CG;
    public CanvasGroup input2CG;
    public GameObject mainPanel;
    public CanvasGroup mainPanelCG;
    public UISecondPanel UISecondPanel;
    public void secondPanelActive()
    {
        Root.Instance.UI.setUnActive(this.mainPanelCG);
        Root.Instance.UI.setActive(UISecondPanel.secondPanelCG);

    }


}
