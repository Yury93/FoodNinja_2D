using UnityEngine;

public class Factory<T> : MonoBehaviour where T : MonoBehaviour
{
   
    public T Create(T prefab,Vector3 position,Transform content, Quaternion rotation)
    {
        var go = Instantiate(prefab,position,rotation,content);
        return go;
    }
}
  