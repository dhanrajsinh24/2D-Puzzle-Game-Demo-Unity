using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup; // Grid parent
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private LevelConfig levelConfig; // Level data will be used from this config
    private Node[,] _nodeGrid; // All nodes in the grid are stored here
    
    private void Start()
    {
        //Auto Initialize
        Initialize();
    }

    private void Initialize()
    {
        levelConfig.Initialize(gridLayoutGroup);

        GenerateGrid();
    }

    //Generate the required grid with all the required nodes at required place
    private void GenerateGrid()
    {
        // Initialize nodeGrid with required size for the current level
        _nodeGrid = new Node[levelConfig.rows, levelConfig.columns];

        for (int i = 0; i < levelConfig.rows; i++)
        {
            for (int j = 0; j < levelConfig.columns; j++)
            {
                var nodeObj = Instantiate(nodePrefab, gridLayoutGroup.transform);
                var node = nodeObj.GetComponent<Node>();
                _nodeGrid[i, j] = node;
                node.Initialize(i, j);
            }
        }
    }
}
