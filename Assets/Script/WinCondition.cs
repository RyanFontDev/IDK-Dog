
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class WinCondition : MonoBehaviour
{
    public GameObject winText;
    public float closeDelay = 3f;

    private bool hasWon = false;

    private void Start()
    {
        if (winText != null)
        {
            winText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon || !other.CompareTag("Player"))
            return;

        hasWon = true;

        if (winText != null)
        {
            winText.SetActive(true);
        }

        StartCoroutine(CloseGame());
    }

    private IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(closeDelay);

        UnityEngine.Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}