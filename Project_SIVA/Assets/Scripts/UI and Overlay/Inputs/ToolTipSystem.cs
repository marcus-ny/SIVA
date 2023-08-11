using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem current;
    public ToolTip toolTip;

    void Awake()
    {
        current = this;
        current.toolTip.gameObject.SetActive(false);
    }

    public static void Show(string content, string header = "")
    {
        current.toolTip.SetText(content, header);
        current.toolTip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.toolTip.gameObject.SetActive(false);
    }
}
