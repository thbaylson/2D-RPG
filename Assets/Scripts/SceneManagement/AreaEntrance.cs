using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    void Start()
    {
        // This works because both SceneManagement and PlayerController are Singletons. As such, they will
        //  both persist through scene changes. Remember, "Instance" is a public static property of Singletons.
        if(transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
        }
    }
}
