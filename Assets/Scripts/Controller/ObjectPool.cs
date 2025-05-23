using UnityEngine;
using System.Collections.Generic;

// singleton quản lý object pool
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag; // tên định danh cho pool
        public GameObject prefab; // prefab để sinh ra object
        public int size; // số lượng object trong pool
    }

    public List<Pool> pools; // danh sách các pool
    private Dictionary<string, Queue<GameObject>> poolDictionary; // lưu trữ các pool theo tag

    private void Awake()
    {
        Instance = this; // khởi tạo singleton
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(pool.tag, objectPool);

            // tạo sẵn các object và đưa vào pool
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }
    }

    // lấy object từ pool, đặt vị trí và xoay, sau đó đưa lại vào pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
