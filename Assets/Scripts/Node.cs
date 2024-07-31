using UnityEngine;

/// <summary>
/// Core script to manage the Node
/// </summary>
public class Node : MonoBehaviour, INode, IRotate
{
    public int RowIndex {get; private set;} // row index of current node (to know it's place)
    public int ColumnIndex {get; private set;} // column index same as row
    private bool _isConnected; //True if this node is connected to atleast one node
    public RectTransform RectTransform {get; private set;}

    public void Initialize(int x, int y)
    {
        RowIndex = x;
        ColumnIndex = y;
        
        RectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        RotateNode();
    }

    public void RotateNode()
    {
        // TODO 
        // Rotate the Node as required
        
        CheckConnections();
    }

    public void CheckConnections()
    {
        // TODO 
        // Check if this node is connected to other nodes
        // Update isConnected status and provide visual feedback
    }
}

internal interface IRotate
{
    void RotateNode();
}

internal interface INode
{
    void Initialize(int x, int y);

    void CheckConnections();
}