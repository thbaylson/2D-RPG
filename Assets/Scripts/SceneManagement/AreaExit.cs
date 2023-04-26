using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    // The name of the entrance we want to appear next to in the next scene.
    [SerializeField] private string sceneTransitionName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene(sceneToLoad);
            // This works becaues SceneManagement is a Singleton that persists through scene transitions.
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        }
    }
}
