using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolBase<T> : MonoBehaviour where T : MonoBehaviour, ISpawnableObject
{
    [SerializeField]
    [Tooltip("오브젝트를 이 개수만큼 초기에 만들어둡니다.")]
    private int initObjectCount = 3;

    [SerializeField]
    private T objectPrefab;

    // 활성화된 오브젝트는 list에, 비활성화된 오브젝트는 queue에 들어있다.
    private List<T> activeObjects = new List<T>();

    private Queue<T> inactiveObjectQueue = new Queue<T>();

    private void Awake()
    {
        for (int i = 0; i < initObjectCount; i++)
        {
            var obj = Instantiate(objectPrefab, transform);
            obj.gameObject.SetActive(false);
            inactiveObjectQueue.Enqueue(obj);
        }
    }

    public T Spawn(Transform parent, ISpawnableObjectInitializeParameter param)
    {
        // 비활성화 큐에 재활용할 수 있는 오브젝트가 있으면 꺼내서 재활용하고, 아니라면 새 활성화 오브젝트를 만들어 반환한다.
        T obj = null;
        if (inactiveObjectQueue.Count > 0)
        {
            obj = inactiveObjectQueue.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = Instantiate(objectPrefab);
            activeObjects.Add(obj);
        }

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }

        obj.Initialize(param);
        return obj;
    }

    public void Despawn(T obj)
    {
        // 비활성화 큐에 오브젝트를 반환한다.
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        activeObjects.Remove(obj);
        inactiveObjectQueue.Enqueue(obj);
    }
}
