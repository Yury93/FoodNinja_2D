using System; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : Factory<SliceTarget>
{
    [Serializable]
    public class SpawnShooter
    {
        public Transform point;
        public Vector3 direction;
    } 
    [SerializeField] private List<SliceTarget> prefabs;
    [SerializeField] private List<SpawnShooter> spawnShooters;
    [SerializeField] private Transform content;
    [SerializeField] private float minTimeSpawn, maxTimeSpawn;
    public List<SliceTarget> targets;
    private float currentTime;
    public Action<SliceTarget> onSlice;
    public static bool IsGameProcess;
    private void Start()
    {
        IsGameProcess = false;
        targets = new List<SliceTarget>();
        //for (int i = 0; i < prefabs.Count; i++)
        //{ 
        //    int rndShooter = UnityEngine.Random.Range(0, spawnShooters.Count);
        //    var spawnShooter = spawnShooters[rndShooter];
        //    var newTarget = Create(prefabs[i], spawnShooter.point.position, content, Quaternion.identity);
        //    newTarget.Init();
        //    newTarget.onSlice += OnSlice;
        //    newTarget.rbMover.SetDirection(spawnShooter.direction);
        //    targets.Add(newTarget);
        //    newTarget.Active();
        //    newTarget.Deactive();
        //}
    }
    private void Update()
    {
        if (IsGameProcess == true)
        {
            if (currentTime < 0)
            {
                SpawnInPool();
                currentTime = UnityEngine.Random.Range(minTimeSpawn, maxTimeSpawn);
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    } 
    private void SpawnInPool()
    { 
        var poolTargets = targets.Where(t => !t.spriteRenderer.enabled == true
        || !t.gameObject.activeSelf && t.SliceType == SliceTarget.SliceName.bomb).ToList();
        if (poolTargets.Count == 0 && prefabs.Count !=0 || prefabs.Count != 0)
        { 
            CreateNewTarget();
        }
        else
        {
            if(poolTargets.Count > 0)
            TakeOfPoolTarget(poolTargets);
        }
    }
    private void TakeOfPoolTarget(List<SliceTarget> poolTargets)
    {
        int rnd = UnityEngine.Random.Range(0, poolTargets.Count);
        SliceTarget target = null; 
        target = poolTargets[rnd]; 
        int rndShooter = UnityEngine.Random.Range(0, spawnShooters.Count);
        var spawnShooter = spawnShooters[rndShooter];
        target.transform.position = spawnShooter.point.position;
        target.rbMover.SetDirection(spawnShooter.direction);
        target.Active();
    }
    private void CreateNewTarget()
    { 
        int rnd = UnityEngine.Random.Range(0, prefabs.Count);
        int rndShooter = UnityEngine.Random.Range(0, spawnShooters.Count);
        var spawnShooter = spawnShooters[rndShooter];
       var prefab = prefabs[rnd];
        var newTarget = Create(prefab , spawnShooter.point.position, content, Quaternion.identity);
        newTarget.Init();
        newTarget.onSlice += OnSlice;
        newTarget.rbMover.SetDirection(spawnShooter.direction);
        targets.Add(newTarget);
        prefabs.Remove(prefab);
        newTarget.Active();
    }

    private void OnSlice(SliceTarget target)
    {
         onSlice.Invoke(target);
    }
    private void OnDestroy()
    {
        IsGameProcess = false;
    }
}
