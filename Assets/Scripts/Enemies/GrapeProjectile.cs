using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float projectilePeak = 3f;
    [SerializeField] private GameObject projectileShadow;
    [SerializeField] private float shadowOffset = -0.3f;

    void Start()
    {
        GameObject shadow = Instantiate(projectileShadow, transform.position + new Vector3(0f, shadowOffset, 0f), Quaternion.identity);
        Vector3 shadowStartPos = shadow.transform.position;

        Vector3 playerPos = PlayerController.Instance.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveShadowRoutine(shadow, shadowStartPos, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0f;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float timeOverDuration = timePassed / duration;
            float heightOverTime = animCurve.Evaluate(timeOverDuration);
            float currentHeight = Mathf.Lerp(0f, projectilePeak, heightOverTime);

            transform.position = Vector2.Lerp(startPos, endPos, timeOverDuration) + new Vector2(0f, currentHeight);

            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator MoveShadowRoutine(GameObject grapeShadow, Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float timeOverDuration = timePassed / duration;

            grapeShadow.transform.position = Vector2.Lerp(startPos, endPos, timeOverDuration);

            yield return null;
        }

        Destroy(grapeShadow);
    }
}
