using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceTarget : MonoBehaviour
{
    public enum SliceName { grape, lemon,tomat,cocos,lime,kiwi, watermelow, bomb,  apple, pinapple,persik,avocado,premium }
    [SerializeField] private float liveTimer;
    [field: SerializeField] public Color colorSlice { get;private set; }
    [field: SerializeField] public RbMover rbMover {  get; private set; }
    [field: SerializeField] public SliceName SliceType { get;private set; }
    [field: SerializeField] public  Collider2D collider2D { get; private set; }
    private float _liveTimer;
     
    public Action<SliceTarget> onSlice;
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }  
    private void OnValidate()
    {
        spriteRenderer = GetComponentInChildren <SpriteRenderer>();
        collider2D = GetComponent <Collider2D>();
    }

    public void Init()
    {
        _liveTimer = liveTimer;
    
    }
     
    public void OnSlice()
    {
        
        onSlice?.Invoke(this);
        Deactive();
    }

    public void Active()
    {
        if (SliceType == SliceTarget.SliceName.bomb) gameObject.SetActive(true);
        else spriteRenderer.enabled = true;
        _liveTimer = liveTimer;
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        //   gameObject.SetActive(true);
    }
    private void Update()
    {
        if (_liveTimer > 0) 
        {
        _liveTimer -= Time.deltaTime;
        }
        else
        {
            Deactive();
        }
    }
    public void Deactive()
    {
 
        if(SliceType == SliceTarget.SliceName.bomb) gameObject.SetActive(false);
        else spriteRenderer.enabled = false;
        collider2D.enabled = false;
        //  gameObject.SetActive(false);
        rbMover.Reset();
    }
}
