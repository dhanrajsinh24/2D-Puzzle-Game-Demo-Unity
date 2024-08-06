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
        private Quaternion _lastTargetRotation;
        [SerializeField] private Transform rotateTransform;
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

        private IEnumerator RotateOverTime(float targetAngle)
        {
            _isRotating = true;

            float elapsedTime = 0f;
            float duration = 0.1f; // Adjust the duration for smoothness
            Quaternion startingRotation = rotateTransform.rotation;
            _lastTargetRotation = Quaternion.Euler(0, 0, targetAngle);

            while (elapsedTime < duration)
            {
                rotateTransform.rotation = Quaternion.Lerp(startingRotation, _lastTargetRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rotateTransform.rotation = _lastTargetRotation;
            _isRotating = false;
        }

        // Rotate the Node by 90 degrees clockwise
        private void RotateBy90()
        {
            if(_lastTargetRotation != null) 
            {
                rotateTransform.rotation = _lastTargetRotation;
            }
            float currentZRotation = rotateTransform.rotation.eulerAngles.z;
            float newZRotation = currentZRotation - 90f; // Rotate counterclockwise by 90 degrees
            _rotateCoroutine = StartCoroutine(RotateOverTime(newZRotation)); 
        }
    }
}
