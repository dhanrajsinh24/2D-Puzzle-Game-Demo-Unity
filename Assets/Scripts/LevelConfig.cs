using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level configuration to be used for the perticular level
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "IG/Level")]
public class LevelConfig : ScriptableObject
{
    public int rows = 5;
    public int columns = 5;
    public float nodeSize = 100f; // Size of each node
    public float spacing = 5f; // Spacing between nodes

    public void Initialize(GridLayoutGroup nodeParentGrid) 
    {
        // Set the Grid layout group as required with the node size spacing etc
        nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
        nodeParentGrid.spacing = new Vector2(spacing, spacing);
        nodeParentGrid.constraintCount = rows;
    }
}
