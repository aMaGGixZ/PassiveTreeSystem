using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLogicInterpreter : MonoBehaviour
{
    List<Node> nodesToRead;

    void Start()
    {
        PassiveTreeManager.st.activeNodesUpdated += ReadNodes;
    }

    void ReadNodes()
    {
        nodesToRead = PassiveTreeManager.st.allocatedNodes;

        foreach (Node n in nodesToRead)
        {
            NodeLogic(n.nodeObject);
        }
    }

    void NodeLogic(NodeSO node)
    {
        switch (node.index)
        {
            case 0: // Str Small
                Debug.Log("+" + node.values[0] + " str");
                break;
            case 1: // Int Small
                Debug.Log("+" + node.values[0] + " int");
                break;
            case 2: // Dex Small
                Debug.Log("+" + node.values[0] + " dex");
                break;
            case 3: // Crit multi
                Debug.Log("+" + node.values[0] + "% crit multi");
                break;
            case 4: // Crit chance
                Debug.Log("+" + node.values[0] + "% crit chance");
                break;
            case 5: // Cold res
                Debug.Log("+" + node.values[0] + "% cold res");
                break;
            case 6: // Str dex
                break;
            case 7: // Int str
                break;
            case 8: // Dex int
                break;
            case 9: // 0
                break;
            case 10: // 0
                break;
            case 11: // 0
                break;
        }
    }
}
