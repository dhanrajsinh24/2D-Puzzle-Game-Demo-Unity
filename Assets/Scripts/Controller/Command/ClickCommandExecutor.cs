using System.Collections;
using System.Collections.Generic;
using IG.Controller;
using UnityEngine;

namespace IG.Command
{
    /// <summary>
    /// Handles the execution and management of click commands, including
    /// queuing, processing, and undoing commands.
    /// </summary>
    public class ClickCommandExecutor : MonoBehaviour
    {
        // Stack to keep track of command history for undo operations.
        private Stack<ICommand> _commandHistory = new ();

        // Queue to manage commands that are waiting to be executed.
        private Queue<ICommand> _commandQueue = new ();
        private bool _isProcessingQueue;
        private ScoreManager _scoreManager;

        public void Initialize(ScoreManager scoreManager) 
        {
            _scoreManager = scoreManager;
        }

        /// <summary>
        /// Creates a new click command for the clicked node and enqueues it for execution.
        /// </summary>
        public void ClickCommand(GameObject clickedNode) 
        {
            var command = new NodeClickCommand(clickedNode, _scoreManager);
            EnqueueCommand(command);
        }

        /// <summary>
        /// Adds a command to the queue and starts processing if not already in progress.
        /// </summary>
        private void EnqueueCommand(ICommand command)
        {
            _commandQueue.Enqueue(command);
            StartCoroutine(ProcessQueue());
        }

        /// <summary>
        /// Processes the command queue, executing commands in order and
        /// maintaining a history for undo operations.
        /// </summary>
        private IEnumerator ProcessQueue()
        {
            if (_isProcessingQueue) yield break;

            _isProcessingQueue = true;

            // Process commands in the queue until it's empty.
            while (_commandQueue.Count > 0)
            {
                var command = _commandQueue.Dequeue();
                yield return command.Execute();
                _commandHistory.Push(command);
                _scoreManager.PlayerMoves++;
            }

            // Mark queue processing as complete.
            _isProcessingQueue = false;
        }

        private void UndoLastNodeClick() 
        {
            if(_commandHistory.Count > 0) 
            {
                var command = _commandHistory.Pop();
                command.Undo();
                _scoreManager.PlayerMoves--;
            }
        }
    }
}
