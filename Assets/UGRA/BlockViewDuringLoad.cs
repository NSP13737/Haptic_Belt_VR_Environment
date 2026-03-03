using UnityEngine;
using System.Collections;

public class BlockViewDuringLoad : MonoBehaviour
{
    // Keeping the delay as a variable makes it easy to adjust in the Inspector
    [SerializeField] private float delay = 3f;

    void Awake()
    {
        // Start the Coroutine as soon as the object is initialized
        StartCoroutine(DisableRoutine());
    }

    private IEnumerator DisableRoutine()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delay);

        // Disable the GameObject this script is attached to
        gameObject.SetActive(false);
    }
}