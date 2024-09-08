using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Raycaster2D : MonoBehaviour
{
    public TouchCollider touchCollider;
    public bool slowMotion;
    public Camera mainCamera;
    public ParticleSystem swordEffect; 
    public static Vector3 startPos;
    public static Vector3 endPos;
    bool process;
    SliceTarget sliceTarget;
    SliceTarget triggerTarget;
    Coroutine coroutine;
    private void Start()
    {
      if(slowMotion)  Time.timeScale = 0.3f;
        touchCollider.onEnterSliceTarget += OnEnterSliceTarget;
        touchCollider.onExit += OnExitSliceTarget;
    }

    private void OnExitSliceTarget()
    {
        endPos = GetMouseWorldPosition(mainCamera, swordEffect.transform);
        if(triggerTarget != null && triggerTarget.gameObject.activeSelf)
            triggerTarget.OnSlice();
    }

    private void OnEnterSliceTarget(SliceTarget target)
    {
        startPos = GetMouseWorldPosition(mainCamera, swordEffect.transform);
        triggerTarget = target;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            touchCollider.cr.enabled = true;
        }
        else
        {
            touchCollider.cr.enabled = false;
        }
        if (Spawner.IsGameProcess)
        {
           
            if (process == false && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                startPos = ray.origin;
                process = true;
            }
            if (process == true && coroutine == null)
            {
                coroutine = StartCoroutine(CorSetEndPosition());
            }
    
        }
        
    }
    private void LateUpdate() 
    {
        if (Spawner.IsGameProcess)
        {
            
            if (Input.GetMouseButton(0))
            {
                var position = GetMouseWorldPosition(mainCamera, swordEffect.transform);
                swordEffect.transform.position = position;
                touchCollider.transform.position = position;
            }
            if (Input.GetMouseButton(0))
            {
                if (swordEffect.gameObject.activeSelf == false) swordEffect.gameObject.SetActive(true);
            }
            else
            {
                if (swordEffect.gameObject.activeSelf == true) swordEffect.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator CorSetEndPosition()
    {
        if (Spawner.IsGameProcess)
        {
            sliceTarget = null;
         
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null)
                {
                    if (sliceTarget == null || sliceTarget.gameObject.activeSelf == false)
                        sliceTarget = hit.collider.gameObject.GetComponent<SliceTarget>();

                }
            }
        

        yield return Time.deltaTime;
        
            if (Input.GetMouseButton(0) && sliceTarget == null ||( sliceTarget != null && sliceTarget.gameObject.activeSelf == false))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null)
                {
                    sliceTarget = hit.collider.gameObject.GetComponent<SliceTarget>();
                }
            }
        
        yield return Time.deltaTime;

            if (Input.GetMouseButton(0) && (sliceTarget == null || sliceTarget != null  && sliceTarget.gameObject.activeSelf == false))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null)
                {
                    if (sliceTarget == null || sliceTarget.gameObject.activeSelf == false)
                    {
                        sliceTarget = hit.collider.gameObject.GetComponent<SliceTarget>();
                    }

                    if (sliceTarget != null)
                    {
                        endPos = sliceTarget.transform.position;
                        sliceTarget.OnSlice();
                    }
                }
            }
       //     debugSprite.transform.localRotation = Quaternion.Euler(GetSliceRotation());
            process = false;
            coroutine = null;
        }
    }

    public static Vector3 GetSliceDirection()
    {
        return (startPos - endPos).normalized;
    }
     public static Vector3 GetSliceRotation()
    {
        var direction = startPos - endPos; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          
        return new Vector3(0,0,angle);
    }
    public static Vector3 GetMouseWorldPosition(Camera mainCamera, Transform target)
    {
        float plainPositionZ = mainCamera.WorldToScreenPoint(target.position).z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, plainPositionZ));
        return worldPosition;
    }
}
