using System.Collections;
using UnityEngine;

public class SlicedObject : MonoBehaviour
{ 
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speedFly,speedRotation;
    [SerializeField] private float timeExplosion;
    [SerializeField] private float gravity;
    [field: SerializeField] public bool isRightSide { get; private set; }
    [field: SerializeField] public Transform sliceTargetTransform { get; private set; }
    [field: SerializeField] public SliceTarget.SliceName sliceName { get; private set; }

    public void Init(SliceTarget sliceTarget, bool isRight)
    {
        sliceTargetTransform = sliceTarget.transform;
        this.isRightSide = isRight;
        StartCoroutine(CorStart()); 
    }

    IEnumerator CorStart()
    {
        rb.gravityScale = 0;
         transform.localRotation = sliceTargetTransform.rotation;
     
        var rotation =  Raycaster2D.GetSliceRotation();
     
        transform.localRotation = Quaternion.Euler(  rotation) ;
        yield return null;
        if (isRightSide) 
        {
            rb.AddForce(transform.up.normalized  * speedFly, ForceMode2D.Impulse);
            rb.AddTorque(-speedRotation, ForceMode2D.Impulse );
        }
        else
        {
            rb.AddForce(-transform.up.normalized * speedFly, ForceMode2D.Impulse);
            rb.AddTorque(speedRotation, ForceMode2D.Impulse);
        } 
        yield return null; 
       Destroy(this.gameObject, 2);
    }
    private void Update()
    {
        if (timeExplosion < 0)
        {
            rb.gravityScale = gravity;
        }
        else
        {
          timeExplosion -= Time.deltaTime;
        } 
    }
}
