using UnityEngine;
using UnityEngine.UI;

public class ShowPointsUI : MonoBehaviour
{
    public PointType type;

    public Text amountText;

    void Start()
    {
        PassiveTreeManager.st.pointsUpdated += UpdateText;
    }

    void UpdateText()
    {
        switch (type)
        {
            case PointType.passive:
                amountText.text = PassiveTreeManager.st.GetPoint(PointType.passive).ToString();
                break;
            case PointType.respec:
                amountText.text = PassiveTreeManager.st.GetPoint(PointType.respec).ToString();
                break;
        }
    }
}