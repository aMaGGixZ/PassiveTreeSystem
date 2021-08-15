using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class NodeConnection
{
    [HideInInspector]
    public string name;
    public Node n1;
    public Node n2;

    public void SetName(string s)
    {
        name = s;
    }
}

[RequireComponent(typeof(PassiveTreeManager))]
public class NodeConnector : MonoBehaviour
{
    public GameObject UILineRendererPrefab;
    public List<NodeConnection> nodeConnections;
    [ReadOnly] public Transform connectionParent;

    void Start()
    {
        ConnectNodes();
    }

    void ConnectNodes()
    {
        UILineRenderer[] children = connectionParent.GetComponentsInChildren<UILineRenderer>(true);
        GetComponent<PassiveTreeManager>().ClearConnectedNodes();

        // "Pooling" - Not needed on a final stable tree version, but good to design / modify it.
        foreach (UILineRenderer c in children)
        {
            c.gameObject.SetActive(false);
        }

        for (int i = 0; i < nodeConnections.Count; i++)
        {
            UILineRenderer lr;

            if (i < children.Length) // Use object from pool
            {
                children[i].gameObject.SetActive(true);
                lr = children[i];
            }
            else // Create object, as pool is too small.
            {
                lr = Instantiate(UILineRendererPrefab, connectionParent).GetComponent<UILineRenderer>();
            }

            // If there's a node missing, remove that connection.
            if (nodeConnections[i].n1 == null || nodeConnections[i].n2 == null)
            {
                nodeConnections.RemoveAt(i);
                i--;
            }
            else // Set connected nodes position to line points.
            {
                nodeConnections[i].n1.connectedNodes.Add(nodeConnections[i].n2);
                nodeConnections[i].n2.connectedNodes.Add(nodeConnections[i].n1);
                lr.Points[0] = nodeConnections[i].n1.GetComponent<RectTransform>().localPosition;
                lr.Points[1] = nodeConnections[i].n2.GetComponent<RectTransform>().localPosition;
                nodeConnections[i].name = nodeConnections[i].n1.nodeObject.name + " <-> " + nodeConnections[i].n2.nodeObject.name;
            }
        }
    }

    void OnDrawGizmos()
    {
        ConnectNodes();
    }
}