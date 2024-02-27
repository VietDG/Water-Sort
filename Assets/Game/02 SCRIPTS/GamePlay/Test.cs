using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Material material;

    float value = 0f;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            value += 0.25f;
            StartCoroutine(SetValue());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            value = -0.5f;
            StartCoroutine(SetValue());
        }
    }

    IEnumerator SetValue()
    {
        float t = 0;
        while (t < 0.5f)
        {
            float newvalue = Mathf.Lerp(-0.5f, value, t / 0.5f);
            material.SetFloat("_FillAmout", newvalue);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
