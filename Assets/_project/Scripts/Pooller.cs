using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooller
{

    public GameObject Prefab;
    Transform _parent;
    public Queue<GameObject> Pool { get; set; } = new Queue<GameObject>();
    private int  MaxOverride;

    public int PoolStartSize = 5;


    public Pooller(int size, GameObject prefab, GameObject poolParent = null)
    {
        PoolStartSize = size;
        MaxOverride = size * 2;
        Prefab = prefab;
        for (int i = 0; i < PoolStartSize; i++)
        {
            GameObject critter;
            if(!poolParent)
            {
                if (LevelSpline.Instance.Pool.transform)
                {
                    critter = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, LevelSpline.Instance.Pool.transform);
                    _parent = LevelSpline.Instance.Pool.transform;
                }
                else
                {
                    critter = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                    _parent = null;
                }
                   
            }
            else
            {
                critter = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, poolParent.transform);
                _parent = poolParent.transform;
            }
              



            critter.name = prefab.name + "-" + i;           
            Pool.Enqueue(critter);
            critter.SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        if (Pool.Count > 0)
        {
            GameObject _tempObject = Pool.Dequeue();
            _tempObject.SetActive(true);
            return _tempObject;
        }
        else if (Pool.Count < MaxOverride)
        {
            GameObject _tempObject = GameObject.Instantiate(Prefab);
            return _tempObject;
        }
        return null;
    }

    public void ReturnToPool(GameObject unpool)
    {
        Pool.Enqueue(unpool);
        unpool.transform.position = Vector3.zero;
        if(_parent)
        {
            unpool.transform.parent = _parent;
        }
      
        unpool.SetActive(false);
    }
}