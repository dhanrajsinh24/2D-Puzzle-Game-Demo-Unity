using IG.Level;
using IG.NodeSystem;
using UnityEngine;
using UnityEngine.UI;

namespace IG.Controller 
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup; // Grid parent
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private LevelConfig levelConfig; // Level data will be used from this config
        private INode[,] _nodeGrid; // All nodes in the grid are stored here
        
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
            _nodeGrid = new INode[levelConfig.Rows, levelConfig.Columns];

            for (int i = 0; i < levelConfig.Rows; i++)
            {
                for (int j = 0; j < levelConfig.Columns; j++)
                {
                    var nodeObj = Instantiate(nodePrefab, gridLayoutGroup.transform);
                    var node = nodeObj.GetComponent<INode>();
                    _nodeGrid[i, j] = node;
                    node.Initialize(i, j);
                }
            }
        }
    }
}
