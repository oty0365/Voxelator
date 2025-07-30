using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PoolObjectType
{
    HpUpAugment,
    FullHpAugment,
    MoveSpeedUpAugment,
    MoreBulletAugment,
    ShiledAugment,
    ExecuteAugment,
    KnifeAugment,
    BoomAugment,
    CloseDealEnemyMove,
    CloseTankEnemyMove,
    RangedEnemyMove,
    DashEnemyMove,
    BulletObject,
    AtkUpAugment,
    DefUpAugment,
    OarAugment,
    HitParticle,
    ExpGiver,
    FlyingBoom,
    PlayerHitParticle,
    FlyingEnemyBoom,
    SFXObject,
    SpeedUpAugment,
}

public class ObjectPooler : HalfSingleMono<ObjectPooler>
{
    [FormerlySerializedAs("retruningParentObj")] [SerializeField] private GameObject returningParentObj;
    public GameObject currentReturningParentObj;
    private Dictionary<PoolObjectType, Queue<GameObject>> objectPoolList;
    private Dictionary<GameObject, IPoolingObject> componentCache;

    protected override void Awake()
    {
        base.Awake();
        InitPoolList();
    }

    public void ReBakeObjectPooler()
    {
        objectPoolList.Clear();
        componentCache.Clear();
    }

    private IPoolingObject GetPoolingComponent(GameObject obj)
    {
        if (!componentCache.TryGetValue(obj, out IPoolingObject poolObj))
        {
            poolObj = obj.GetComponent<IPoolingObject>();
            componentCache[obj] = poolObj;
        }
        return poolObj;
    }

    private PoolObjectType GetObjectType(GameObject obj)
    {
        var typeDefiner = obj.GetComponent<ObjectTypeDefiner>();
        if (typeDefiner != null)
        {
            return typeDefiner.poolObjectType;
        }
        
        Debug.LogError($"ObjectTypeDefiner component not found on {obj.name}");
        return PoolObjectType.HpUpAugment;
    }

    public GameObject Get(GameObject prefab, Vector2 position, Vector3 rotation)
    {
        PoolObjectType key = GetObjectType(prefab);
        
        if (!objectPoolList.ContainsKey(key))
        {
            objectPoolList[key] = new Queue<GameObject>();
        }

        GameObject obj;
        IPoolingObject poolObj;

        if (objectPoolList[key].Count == 0)
        {
            obj = Instantiate(prefab, position, Quaternion.Euler(rotation));
            poolObj = GetPoolingComponent(obj);
        }
        else
        {
            obj = objectPoolList[key].Dequeue();
            poolObj = GetPoolingComponent(obj);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(rotation);
            obj.SetActive(true);
        }

        obj.transform.SetParent(currentReturningParentObj.transform, worldPositionStays: false);
        obj.SetActive(true);
        poolObj.OnBirth();
        
        return obj;
    }

    public GameObject Get(GameObject prefab, Vector2 position, Vector3 rotation, Vector2 size)
    {
        PoolObjectType key = GetObjectType(prefab);

        if (!objectPoolList.ContainsKey(key))
            objectPoolList[key] = new Queue<GameObject>();

        GameObject obj;
        IPoolingObject poolObj;

        if (objectPoolList[key].Count == 0)
        {
            obj = Instantiate(prefab, position, Quaternion.Euler(rotation));
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);
            poolObj = GetPoolingComponent(obj);
        }
        else
        {
            obj = objectPoolList[key].Dequeue();
            poolObj = GetPoolingComponent(obj);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(rotation);
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);
            obj.SetActive(true);
        }

        obj.transform.SetParent(currentReturningParentObj.transform, worldPositionStays: false);
        poolObj.OnBirth();
        
        return obj;
    }

    public GameObject Get(GameObject prefab, Transform parent)
    {
        PoolObjectType key = GetObjectType(prefab);

        if (!objectPoolList.ContainsKey(key))
            objectPoolList[key] = new Queue<GameObject>();

        GameObject obj;
        IPoolingObject poolObj;

        if (objectPoolList[key].Count == 0)
        {
            obj = Instantiate(prefab, parent);
            poolObj = GetPoolingComponent(obj);
        }
        else
        {
            obj = objectPoolList[key].Dequeue();
            poolObj = GetPoolingComponent(obj);
            obj.transform.parent = parent;
            obj.SetActive(true);
        }

        poolObj.OnBirth();
        return obj;
    }

    public void Return(GameObject obj)
    {
        PoolObjectType key = GetObjectType(obj);

        if (!objectPoolList.ContainsKey(key))
            objectPoolList[key] = new Queue<GameObject>();
        
        var poolObj = GetPoolingComponent(obj);
        componentCache[obj] = poolObj;
        poolObj.OnDeathInit();
        obj.SetActive(false);
        obj.transform.SetParent(currentReturningParentObj.transform, worldPositionStays: false);
        objectPoolList[key].Enqueue(obj);
    }

    private void InitPoolList()
    {
        if (currentReturningParentObj != null)
            Destroy(currentReturningParentObj);
        
        currentReturningParentObj = Instantiate(returningParentObj);
        objectPoolList = new Dictionary<PoolObjectType, Queue<GameObject>>();
        componentCache = new Dictionary<GameObject, IPoolingObject>();
    }
}