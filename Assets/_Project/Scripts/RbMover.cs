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
    private Vector3 rndRotation;
    private bool startCorPremiumSliceTarget;
    //private void OnValidate()
    //{
    //    if (sliceTarget == null)
    //        sliceTarget = GetComponent<SliceTarget>();
    //}
    private void Start()
    {
        var rndOperator = UnityEngine.Random.Range(0, 3); 
        var z = UnityEngine.Random.Range(150, 150);

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
    }
    private void FixedUpdate()
    {
        if (transform.position.y < jump && rb.gravityScale == 0)
        {
            rb.AddForce(moveDirection.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            if (sliceTarget.SliceType != SliceTarget.SliceName.premium)
                rb.gravityScale= gravityScale;
            //else if (startCorPremiumSliceTarget == false)
            //{
            //    rb.AddForce(-rb.velocity, ForceMode2D.Impulse);
            //    startCorPremiumSliceTarget = true;
               
            //}
        }
    }
    public void Reset()
    {
        rb.velocity = new Vector2 (0, 0);
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
    }
}
