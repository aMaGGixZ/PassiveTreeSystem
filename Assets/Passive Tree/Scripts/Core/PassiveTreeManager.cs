using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PointType { passive, respec }

public class PassiveTreeManager : MonoBehaviour
{
    public static PassiveTreeManager st;

    public delegate void Call();
    public Call pointsUpdated;
    public Call activeNodesUpdated;

    [SerializeField] int _passivePoints;
    int PassivePoints
    {
        get { return _passivePoints; }
        set
        {
            _passivePoints = value;
            pointsUpdated?.Invoke();
        }
    }
    [SerializeField] int _respecPoints;
    int RespecPoints
    {
        get { return _respecPoints; }
        set
        {
            _respecPoints = value;
            pointsUpdated?.Invoke();
        }
    }
    public NodeDB nodeDB;
    public List<Node> allocatedNodes; // All alocated nodes (not counting starting nodes)

    [ReadOnly, SerializeField] private List<Node> containedNodes; // Every node in the tree.
    [ReadOnly] public Transform nodeParentUI;
    private List<Node> activeNodes = new List<Node>(); // All allocated nodes + starting nodes.

    void Awake()
    {
        if (st != null) Destroy(st);
        st = this;
    }

    void Start()
    {
        SetContainedNodes();
        InitializeTree();
        UpdateActiveNodes();
        pointsUpdated?.Invoke();
    }

    // Sets the tree to its initial state (all empty)
    void InitializeTree()
    {
        foreach (Node n in containedNodes)
        {
            n.SetState(false);
        }
    }

    // Called from NodeConnector.cs
    public void ClearConnectedNodes()
    {
        foreach (Node n in containedNodes)
        {
            n.connectedNodes.Clear();
        }
    }

    // Called by nodes, after being activated/deactivated
    public void UpdateActiveNodes()
    {
        activeNodes.Clear();
        foreach (Node n in containedNodes)
        {
            if (n.IsActive) activeNodes.Add(n);
        }

        allocatedNodes.Clear();
        foreach (Node n in activeNodes)
        {
            if (n.type == NodeType.normal)
            {
                allocatedNodes.Add(n);
                n.SetEnabledImage();
            }
        }

        activeNodesUpdated?.Invoke();
    }

    public bool ConsumePoint(PointType pT)
    {
        switch (pT)
        {
            case PointType.passive:
                if(PassivePoints > 0)
                {
                    PassivePoints--;
                    return true;
                }             
                break;
            case PointType.respec:
                if (RespecPoints > 0)
                {
                    RespecPoints--;
                    PassivePoints++;
                    return true;
                }
                break;
        }

        return false;
    }

    public void AddPoint(PointType pT)
    {
        switch (pT)
        {
            case PointType.passive:
                PassivePoints++;
                break;
            case PointType.respec:
                RespecPoints++;
                break;
        }
    }

    public int GetPoint(PointType pT)
    {
        int amount = 0;
        switch (pT)
        {
            case PointType.passive:
                amount = PassivePoints;
                break;
            case PointType.respec:
                amount = RespecPoints;
                break;
        }
        return amount;
    }

    public void AddPassive()
    {
        AddPoint(PointType.passive);
    }
    public void AddRespec()
    {
        AddPoint(PointType.respec);
    }
    // Only useful for design purposes. Not going to be used on the build.
    void SetContainedNodes()
    {
        containedNodes.Clear();
        foreach (Node n in nodeParentUI.GetComponentsInChildren<Node>())
        {
            containedNodes.Add(n);
            n.SetSprite();
        }
    }

    void OnDrawGizmos()
    {
        nodeDB.SetNodeIndex();
        SetContainedNodes();
    }
}