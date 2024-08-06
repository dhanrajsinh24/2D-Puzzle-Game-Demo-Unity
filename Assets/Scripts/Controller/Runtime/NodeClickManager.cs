using IG.NodeSystem;
using TMPro;
using UnityEngine;

namespace IG.Controller
{
    public class NodeClickManager : MonoBehaviour
    {
        private int _layerMask;
        private bool _isClickable;
        private ScoreManager _scoreManager;
        private float _lastTouchTime; // Tracks the last time a touch input was registered
        private const float _touchCooldown = 0.15f; // The cooldown time in seconds

        public void Initialize(ScoreManager scoreManager) 
        {
            _scoreManager = scoreManager;

            // Set up a layer mask to only include the "Node" layer
            int nodeLayer = LayerMask.NameToLayer("Node");
            _layerMask = 1 << nodeLayer;
        }

        private void OnEnable() 
        {
            CircuitValidation.OnValidated += StopNodeClick;
            LevelManager.OnLevelLoaded += StartNodeClick;
        }

        private void OnDisable() 
        {
            CircuitValidation.OnValidated -= StopNodeClick;
            LevelManager.OnLevelLoaded -= StartNodeClick;
        }

        private void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if(!_isClickable) return;

            // Check if the cooldown period has passed since the last touch
            if (Time.time - _lastTouchTime < _touchCooldown) return;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    
                    // Perform a raycast that only hits objects on the "Node" layer
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerMask);

                    if (hit.collider != null)
                    {
                        _lastTouchTime = Time.time; // Update the last touch time
                        NodeClicked(hit.collider.gameObject);
                    }
                }
            }

            #if UNITY_EDITOR

            // Handle mouse input for editor/testing purposes
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Perform a raycast that only hits objects on the "Node" layer
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _layerMask);

                if (hit.collider != null)
                {
                    _lastTouchTime = Time.time; // Update the last touch time
                    NodeClicked(hit.collider.gameObject);
                }
            }

            #endif
        }

        private void NodeClicked(GameObject clickedNode)
        {
            clickedNode.GetComponent<Node>().NodeClicked();
            _scoreManager.PlayerMoves++;
        }

        private void StartNodeClick(int _, int __)
        {
            // Start clicking of nodes
            _isClickable = true;
        }

        private void StopNodeClick()
        {
            // Stop clicking of nodes
            _isClickable = false;
        }
    }
}
