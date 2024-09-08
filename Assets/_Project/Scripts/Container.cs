using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [field: SerializeField] public Spawner Spawner {  get; private set; }
    [field: SerializeField] public SpawnSliceEffects SpawnerSlicedObjects { get; private set; }
    public static Container instance;
    private void Awake()
    {
        instance = this;
    }
}
