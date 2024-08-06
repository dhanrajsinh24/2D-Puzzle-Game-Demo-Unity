using System.Collections;
using IG.Controller;
using UnityEngine;

namespace IG.Utils 
{
	public class CameraShake : MonoBehaviour
{
	[SerializeField]private Transform cameraTransform; // Transform of the camera to shake
	private const float ShakeDuration = 0.2f; // How long the object should shake for
	private const float ShakeAmount = 0.1f; // Magnitude of the shake. A larger value shakes the camera harder

	private void Awake()
	{
		if (!cameraTransform)
		{
			cameraTransform = transform;
		}
	}

	private void OnEnable() 
	{
		LevelManager.OnLevelCompleted += StartShake;
	}

	private void OnDisable() 
	{
		LevelManager.OnLevelCompleted -= StartShake;
	}

	private void StartShake(int _, int __) 
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