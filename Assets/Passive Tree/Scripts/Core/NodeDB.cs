using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node DB", menuName = "Passive Tree/Node Database")]
public class NodeDB : ScriptableObject
{
    public List<NodeSO> nodes;

    // Only useful for design purposes. Not going to be used on the build.
    public void SetNodeIndex()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].index = i;
        }
    }
}