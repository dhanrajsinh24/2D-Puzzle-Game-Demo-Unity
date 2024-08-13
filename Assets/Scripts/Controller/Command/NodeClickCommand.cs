using UnityEngine;
using IG.NodeSystem;
using IG.Controller;
using System.Collections;

namespace IG.Command
{
    /// <summary>
    /// Represents a command for handling node click actions, including
    /// execution and undo functionality.
    /// </summary>
    public class NodeClickCommand : ICommand
    {
        private readonly GameObject _clickedNode;
        private readonly ScoreManager _scoreManager;

        public NodeClickCommand(GameObject clickedNode, ScoreManager scoreManager)
        {
            _clickedNode = clickedNode;
            _scoreManager = scoreManager;
        }

        /// <summary>
        /// Executes the command, triggering the node's click behavior
        /// </summary>
        public IEnumerator Execute()
        {
            yield return _clickedNode.GetComponent<Node>().NodeClicked();
            
            // Update player's move count.
            _scoreManager.PlayerMoves++;
        }

        public void Undo()
        {
            // TODO 
            //_scoreManager.PlayerMoves--;
            
        }
    }
}

