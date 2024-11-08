using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolBase<T> : MonoBehaviour where T : MonoBehaviour, ISpawnableObject
{
    [SerializeField]
    [Tooltip("������Ʈ�� �� ������ŭ �ʱ⿡ �����Ӵϴ�.")]
    private int initObjectCount = 3;

    [SerializeField]
    private T objectPrefab;

    // Ȱ��ȭ�� ������Ʈ�� list��, ��Ȱ��ȭ�� ������Ʈ�� queue�� ����ִ�.
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
        // ��Ȱ��ȭ ť�� ��Ȱ���� �� �ִ� ������Ʈ�� ������ ������ ��Ȱ���ϰ�, �ƴ϶�� �� Ȱ��ȭ ������Ʈ�� ����� ��ȯ�Ѵ�.
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
        // ��Ȱ��ȭ ť�� ������Ʈ�� ��ȯ�Ѵ�.
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        activeObjects.Remove(obj);
        inactiveObjectQueue.Enqueue(obj);
    }
}
