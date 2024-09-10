using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbMover : MonoBehaviour
{
    [SerializeField] private SliceTarget sliceTarget;
    [SerializeField] private float jump;
    [SerializeField] private float minJump, maxJump;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float speed;
    [SerializeField] private float gravityScale;
    public float addForceTime;
    private Vector3 rndRotation;
    private bool startCorPremiumSliceTarget;
    private float startTimer;
    //private void OnValidate()
    //{
    //    if (sliceTarget == null)
    //        sliceTarget = GetComponent<SliceTarget>();
    //}
    private void Start()
    {
        var rndOperator = UnityEngine.Random.Range(0, 3); 
        var z = UnityEngine.Random.Range(150, 150);
        startTimer = addForceTime;
        if (rndOperator == 2)
        {
            z = -z;
        }
        //if (sliceTarget.SliceType == SliceTarget.SliceName.premium)
        //{
             
        //    z = 0;
        //}
        rndRotation = new Vector3(0, 0, z);
     
    }
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
        jump = UnityEngine.Random.Range(minJump , maxJump);
    }
    private void Update()
    {
        transform.Rotate(rndRotation * Time.deltaTime);
        if (/*transform.position.y < jump && */ gameObject.activeSelf && sliceTarget.spriteRenderer.enabled == true && rb.gravityScale == 0 && addForceTime > 0)
        {
            rb.velocity = moveDirection.normalized * speed; // Изменяем скорость напрямую
            addForceTime -= Time.deltaTime;
        }
        else
        {
            if (sliceTarget.SliceType != SliceTarget.SliceName.premium && rb.gravityScale == 0)
            {
              
                rb.gravityScale = gravityScale;
                speed = UnityEngine.Random.Range(7.5F, 9F);
            }
            addForceTime = startTimer;
        }
        if(rb.transform.position.y < -6  && (sliceTarget.SliceType == SliceTarget.SliceName.bomb && !gameObject.activeSelf || sliceTarget.SliceType != SliceTarget.SliceName.bomb && sliceTarget.spriteRenderer.enabled == false))
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
 //   private void FixedUpdate()
  //  {

        //if (transform.position.y < jump && rb.gravityScale == 0 && addForceTime > 0)
        //{
        //    rb.velocity = moveDirection.normalized * speed; // Изменяем скорость напрямую
        //    addForceTime -= Time.fixedDeltaTime;
        //}
        //else
        //{
        //    if (sliceTarget.SliceType != SliceTarget.SliceName.premium && rb.gravityScale == 0)
        //        rb.gravityScale = gravityScale;

            //if (transform.position.y < jump && rb.gravityScale == 0  && addForceTime > 0)
            //{
            //    rb.AddForce(moveDirection.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            //    addForceTime -= Time.fixedDeltaTime;
            //}
            //else
            //{
            //    if (sliceTarget.SliceType != SliceTarget.SliceName.premium && rb.gravityScale == 0)
            //        rb.gravityScale = gravityScale;
            //    //else if (startCorPremiumSliceTarget == false)
            //    //{
            //    //    rb.AddForce(-rb.velocity, ForceMode2D.Impulse);
            //    //    startCorPremiumSliceTarget = true;

            //    //}
            //    addForceTime = startTimer;
            //}
        //}
   // }
    public void Reset()
    {
        rb.velocity = new Vector2 (0, 0);
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
    }
}
