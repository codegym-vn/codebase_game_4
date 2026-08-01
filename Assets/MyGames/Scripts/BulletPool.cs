using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public  GameObject bulletPrefab;
    public int poolSize = 30;
    private List<GameObject> pool = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private static BulletPool instance;
    public static BulletPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<BulletPool>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("BulletPool");
                    instance = obj.AddComponent<BulletPool>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return  pool[i];
        }
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        pool.Add(bullet);
        return bullet;
    }
}
