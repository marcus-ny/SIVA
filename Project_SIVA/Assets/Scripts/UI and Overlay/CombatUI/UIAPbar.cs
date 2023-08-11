using UnityEngine;
using UnityEngine.UI;

public class UIAPbar : MonoBehaviour
{
    Slider _actionpointSlider;
    [SerializeField] UIAPbar APbar;

    private void Start()
    {
        _actionpointSlider = GetComponent<Slider>();
    }

    void Update()
    {
        int max = BattleSimulator.Instance.MAX_ACTIONS;
        int curr = BattleSimulator.Instance.actionsPerformed;
        APbar.SetAP(max - curr);
    }

    public void SetMaxAP(int maxActionPoint)
    {
        _actionpointSlider.maxValue = maxActionPoint;
        _actionpointSlider.value = maxActionPoint;
    }

    public void SetAP(int actionPoint)
    {
        _actionpointSlider.value = actionPoint;
    }
}
