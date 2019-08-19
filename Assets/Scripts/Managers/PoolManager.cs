using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class DictClonedPrefabs : SerializableDictionaryBase<GameObjectType, GameObject> { }

public class PoolContainer : List<GameObject>
{
    public GameObject getEmptyObject()
    {
        GameObject result = null;
        foreach (GameObject _obj in this)
        {

            if (!_obj.activeSelf)
            {
                result = _obj;
                break;
            }
        }
        return result;
    }
    // Насамом деле обьект не удаляется а делается не активным и готов быть снова назначен
    public void Release()
    {
        foreach (GameObject _obj in this)
        {
            if (_obj.activeSelf)
            {
                _obj.SetActive(false);
            }
        }
    }
}

public class PoolManager : MonoBehaviour
{

    public static PoolManager instance = null;

    public DictClonedPrefabs ClonedPrefabs;

    [SerializeField]
    private Dictionary<GameObjectType, PoolContainer> poolObjects = new Dictionary<GameObjectType, PoolContainer>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (instance == this)
        {
            Destroy(gameObject); // Удаляем объект
        }

        DontDestroyOnLoad(gameObject);
    }

    public GameObject getPoolObject(GameObjectType type_obj)
    {
        GameObject result = null;
        if (!poolObjects.ContainsKey(type_obj))
        {
            poolObjects[type_obj] = new PoolContainer();
        }
        else
        {
            result = poolObjects[type_obj].getEmptyObject();
        }
        if (result == null)
        {
            result = InstanceObject(type_obj);
        }

        return result;
    }
    /// <summary>
    /// Удаляем всех из очередей и их деактивируем
    /// </summary>
    public void Release()
    {
        foreach(GameObjectType key in poolObjects.Keys)
        {
            poolObjects[key].Release();
        }
        poolObjects.Clear();
    }
    private GameObject InstanceObject(GameObjectType type_obj)
    {
        if (!ClonedPrefabs.ContainsKey(type_obj))
        {
            Debug.LogError("Not found prefab " + type_obj);
            return null;
        }
        GameObject result = Instantiate(ClonedPrefabs[type_obj], Vector3.zero, Quaternion.identity);
        result.gameObject.SetActive(false);
        poolObjects[type_obj].Add(result);
        return result;
    }
}
