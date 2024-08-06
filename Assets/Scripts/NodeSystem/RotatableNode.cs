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
            float duration = 0.2f; // Adjust the duration for smoothness
            Quaternion startingRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            while (elapsedTime < duration)
            {
                transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            _isRotating = false;
        }

        // Rotate the Node by 90 degrees clockwise
        private void RotateBy90()
        {
            float currentZRotation = transform.rotation.eulerAngles.z;
            float newZRotation = currentZRotation - 90f; // Rotate counterclockwise by 90 degrees
            _rotateCoroutine = StartCoroutine(RotateOverTime(newZRotation)); 
        }
    }
}
