using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCollider : MonoBehaviour
{
  [field: SerializeField]  public Collider2D cr {  get; private set; }
    public Action<SliceTarget> onEnterSliceTarget;
    public Action  onExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Spawner.IsGameProcess)
        {
            var sliceTarget = collision.GetComponent<SliceTarget>();
            if (sliceTarget != null && sliceTarget.gameObject.activeSelf)
            {
                onEnterSliceTarget(sliceTarget);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Spawner.IsGameProcess)
        {
            var sliceTarget = collision.GetComponent<SliceTarget>();
            if (sliceTarget != null && sliceTarget.gameObject.activeSelf)
            {
                onExit?.Invoke();
            }
        }
    }
}
