using System.Collections;
using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Script enables the node to Rotate on click
    /// </summary>
    public class RotatableNode : Node, IRotatable
    {
        private bool _isRotating;
        private Coroutine _rotateCoroutine;
        private float _lastTargetZRotation;

        public override void NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            Rotate();
        }

        public void Rotate()
        {
            RotateBy90();

            ShiftConnectibleSides();

            CheckConnections();
        }

        private IEnumerator RotateOverTime(float targetZRotation)
        {
            _isRotating = true;

            float elapsedTime = 0f;
            float duration = 0.1f; // Adjust the duration for smoothness
            Vector3 startingEulerAngles = rotateTransform.eulerAngles;
            _lastTargetZRotation = targetZRotation;
            Debug.Log($"{gameObject.name}, targetZRotation {targetZRotation}");

            while (elapsedTime < duration)
            {
                float newZRotation = Mathf.Lerp(startingEulerAngles.z, _lastTargetZRotation, elapsedTime / duration);
                rotateTransform.eulerAngles = new Vector3(startingEulerAngles.x, startingEulerAngles.y, newZRotation);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rotateTransform.eulerAngles = new Vector3(startingEulerAngles.x, startingEulerAngles.y, _lastTargetZRotation);
            _isRotating = false;
        }

        // Rotate the Node by 90 degrees clockwise
        private void RotateBy90()
        {
            if (_rotateCoroutine != null) 
            {
                StopCoroutine(_rotateCoroutine);
            }

            float currentZRotation = rotateTransform.eulerAngles.z;
            float newZRotation = currentZRotation - 90f; // Rotate counterclockwise by 90 degrees
            _rotateCoroutine = StartCoroutine(RotateOverTime(newZRotation)); 
        }
    }
}
