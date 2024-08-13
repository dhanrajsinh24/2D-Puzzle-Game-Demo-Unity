using System.Collections;
using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Extends Node to add rotation functionality. 
    /// Handles updating connections when the node is rotated.
    /// </summary>
    public class RotatableNode : Node, IRotatable
    {
        private Coroutine _rotateCoroutine;
        private float _lastTargetZRotation;

        public override IEnumerator NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            yield return Rotate();
        }

        public IEnumerator Rotate()
        {
            yield return RotateBy90();

            ShiftConnectibleSides();

            CheckConnections();
        }

        private IEnumerator RotateOverTime(float targetZRotation)
        {
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
        }

        // Rotate the Node by 90 degrees clockwise
        private IEnumerator RotateBy90()
        {
            if (_rotateCoroutine != null) 
            {
                StopCoroutine(_rotateCoroutine);
            }

            float currentZRotation = rotateTransform.eulerAngles.z;
            
            // Rotate counterclockwise by 90 degrees
            float newZRotation = currentZRotation - 90f; 
            yield return RotateOverTime(newZRotation);
        }
    }
}
