using System.Collections;
using UnityEngine;

namespace IG.Utils 
{
	public class CameraShake : MonoBehaviour
{
	[SerializeField]private Transform cameraTransform; // Transform of the camera to shake
	private const float ShakeDuration = 0.5f; // How long the object should shake for
	private const float ShakeAmount = 0.7f; // Magnitude of the shake. A larger value shakes the camera harder

	private void Awake()
	{
		if (!cameraTransform)
		{
			cameraTransform = transform;
		}
	}

	public void StartShake() 
	{
		StartCoroutine(Shake(ShakeDuration, ShakeAmount));
	}

	private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * magnitude;

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}
}