using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public void SetPosition(Vector2 target)
    {
        this.transform.position = target;
    }

    public void Movement(Vector2 target)
    {
        this.gameObject.SetActive(false);
        SetPosition(target);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        SimplePool.Despawn(this.gameObject);
    }
}
