using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CartoonFX;
using System.Collections;

public class SpawnSliceEffects : MonoBehaviour
{
    [Serializable]
    public class SliceObjectPart
    {
        public SlicedObject slicedObject1, slicedObject2;
    }
    [SerializeField] private CFXR_Effect heartEffect;
    [SerializeField] private CFXR_Effect explosionEffect;
    [SerializeField] private BlowEffect blowEffect;
    [SerializeField] private List<SliceObjectPart> slicedObjectPrefabs;
    [SerializeField] private GameObject[] slashPrefabs;
    private void Start()
    {
        Container.instance.Spawner.onSlice += OnSliced;
    }

    private void OnSliced(SliceTarget target)
    {
        CreateSlashEffect(target);
        if (target.SliceType != SliceTarget.SliceName.bomb)
        {
            var blow = Instantiate(blowEffect, target.transform.position, Quaternion.identity);
            blow.SetColor(target.colorSlice);

            var slicedObjectPrefab = slicedObjectPrefabs.FirstOrDefault(s => s.slicedObject1.sliceName == target.SliceType);
            if (slicedObjectPrefab != null)
            {
                var slicedObj = Instantiate(slicedObjectPrefab.slicedObject1, target.transform.position, Quaternion.identity);
                slicedObj.Init(target, true);
                var slicedObj2 = Instantiate(slicedObjectPrefab.slicedObject2, target.transform.position, Quaternion.identity);
                slicedObj2.Init(target, false);
            }
        }
        else
        {
            var bombEffect = Instantiate(explosionEffect, target.transform.position, Quaternion.identity);
            Destroy(bombEffect.gameObject, 2f);
            StartCoroutine(corCreateHeart());
            IEnumerator corCreateHeart()
            {
                yield return new WaitForSeconds(0.5f);
                var bombEffect = Instantiate(heartEffect, new Vector3 (0,-2,0), Quaternion.identity);
                Destroy(bombEffect.gameObject, 2f);
            }
        }

    }
    private void CreateSlashEffect(SliceTarget target)
    {
        var rndSlashEffect = UnityEngine.Random.Range(0, slashPrefabs.Length);
        var slashEffect = Instantiate(slashPrefabs[rndSlashEffect], target.transform.position, target.transform.rotation);
        Destroy(slashEffect.gameObject, 2f);
    }
}
