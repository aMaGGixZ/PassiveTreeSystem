using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node", menuName = "Passive Tree/Node")]
public class NodeSO : ScriptableObject
{
    public Sprite icon;
    [Tooltip("Write {x}, \"x\" being the value index to replace from the \"values\" array.")]
    [TextArea, SerializeField] string tooltip;
    public int[] values;
    [ReadOnly] public int index;

    public string GetTooltip()
    {
        string finalTooltip = tooltip;

        for (int i = 0; i < values.Length; i++)
        {
            finalTooltip = finalTooltip.Replace("{" + i + "}", values[i].ToString());
        }

        return finalTooltip;
    }
}