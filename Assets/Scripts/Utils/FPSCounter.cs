using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counterText;

        private void Start() => StartCoroutine(Count());

        private IEnumerator Count()
        {
            var count = 0;
            for (var t = 0f; t < 1; t += Time.deltaTime, count++)
                yield return null;

            counterText.SetText($"{count}");
            StartCoroutine(Count());
        }

    }
}
