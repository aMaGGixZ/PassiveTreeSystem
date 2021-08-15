using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NodeType { normal, starting }

public class Node : MonoBehaviour
{
    public NodeSO nodeObject;
    [Header("---- Read only attributes ----")]
    [ReadOnly] public NodeType type;
    [ReadOnly] public Image imageUI;
    [ReadOnly] public GameObject disabledImg;
    [ReadOnly] public List<Node> connectedNodes;
    public bool IsActive
    {
        get { return _active; }
    }
    [ReadOnly, SerializeField] bool _active;

    int maxIterations = 100;

    public void ToggleState()
    {
        bool wantedState = !IsActive;
        if (wantedState) // Want to allocate
        {
            if (!CanAllocateNode())
            {
                Debug.Log("Can't allocate: There are no connected nodes nearby");
                return;
            }
            if (!PassiveTreeManager.st.ConsumePoint(PointType.passive))
            {
                Debug.Log("Can't allocate: Not enough passive points");
                return;
            }
        }
        else // Want to unallocate
        {
            if (!CanUnallocateNode())
            {
                Debug.Log("Can't unallocate: Breaks the chain connected to the starting point");
                return;
            }
            if (!PassiveTreeManager.st.ConsumePoint(PointType.respec))
            {
                Debug.Log("Can't unallocate: Not enough respec points");
                return;
            }
        }

        _active = wantedState;
        PassiveTreeManager.st.UpdateActiveNodes();
        SetEnabledImage();
    }

    bool CanAllocateNode()
    {
        foreach (Node n in connectedNodes)
        {
            if (n.IsActive)
            {
                //Debug.Log(nodeObject.GetTooltip());
                return true;
            }
        }

        return false;
    }

    bool CanUnallocateNode()
    {
        List<Node> checkedNodes;
        Queue<Node> nodeQueue;
        Node currentNode;
        int currentIterations;
        bool couldUnallocate = true;
        bool isStartingNode;
      
        foreach (Node n in connectedNodes) // Do X searches based on the amount of connected nodes.
        {
            checkedNodes = new List<Node>();
            checkedNodes.Add(this);
            nodeQueue = new Queue<Node>();
            currentNode = n;
            currentIterations = 0;
            isStartingNode = false;

            // Breadth first search method.
            while (currentIterations < maxIterations)
            {
                int validNodes = 0;
                currentIterations++;

                if (currentNode.type == NodeType.starting) break; // Ignore path if first connected node is the starting node.
                if (!currentNode.IsActive) break; // Ignore path if first connected node is an inactive node.

                foreach (Node sn in currentNode.connectedNodes) // Check connected nodes
                {
                    if (sn.type == NodeType.starting) // If it's connected to the start, stop checking. Can unallocate.
                    {
                        isStartingNode = true;
                        break;
                    }
                    if (checkedNodes.Contains(sn)) continue; // If that node is contained on checkedNodes list, don't add it to the queue.
                    if (!sn.IsActive) continue; // If that node is not active, don't add it to the queue.

                    nodeQueue.Enqueue(sn); // Enqueue the connected node.
                    validNodes++;
                }

                if (isStartingNode) // If it's connected to the start, stop iterating. Can unallocate.
                {
                    couldUnallocate = true;
                    break;
                }

                if (validNodes == 0) // If none of the connected nodes was valid, it's an end point. Set couldUnallocate to false and keep iterating through other nodes on the queue.
                {
                    couldUnallocate = false;
                }

                checkedNodes.Add(currentNode); // Add the checked node to the checkedNodes list.
                if(nodeQueue.Count > 0) // If there are nodes in the queue to check, set the first as the currentNode, and repeat the check process.
                {
                    currentNode = nodeQueue.Dequeue();
                }
            } // End of iterations (maximum marked by maxIterations).

            if (!couldUnallocate) break;
        } // End of 1 search (1 by connected node to this node).

        return couldUnallocate;
    }

    public void SetSprite()
    {
        if (nodeObject != null)
        {
            imageUI.sprite = nodeObject.icon;
        }
    }

    public void SetState(bool state)
    {
        if (type == NodeType.starting) return;
        _active = state;
    }

    public void SetEnabledImage()
    {
        disabledImg.SetActive(!IsActive);
    }
}