using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private int size = 0;
    [SerializeField] private bool fixedSize = true;
    private int count = 0;

    private List<GameObject> storedObjects;

    public int Size
    {
        get { return size; }
    }

    private void InitStoredObjects()
    {
        storedObjects = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            var go = InstantiateNewObject();
            storedObjects.Add(go);
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadPrefabFromAssetBundle());

        InitStoredObjects();
    }

    public IEnumerator LoadPrefabFromAssetBundle()
    {
        var bundleUrl = string.Format("file:///{0}/{1}", Application.dataPath, "AssetBundles/circlebundle");

        using (WWW www = new WWW(bundleUrl))
        {
            yield return www;

            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);

            AssetBundle bundle = www.assetBundle;
            prefab = bundle.LoadAsset<GameObject>("Circle");
            bundle.Unload(false);
        }
    }

    private GameObject InstantiateNewObject()
    {
        GameObject go = Instantiate(prefab);
        count += 1;
        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localScale = parent.transform.localScale;
        }
        go.name = prefab.name + "_" + count;
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject()
    {
        GameObject go = null;
        if (storedObjects.Count > 0)
        {
            go = storedObjects[0];
            storedObjects.RemoveAt(0);
        }
        else
        {
            if (!fixedSize)
            {
                go = InstantiateNewObject();
            }
        }
        return go;
    }

    public void ReturnObject(GameObject go)
    {
        go.SetActive(false);
        if (storedObjects.Contains(go))
        {
            return;
        }
        storedObjects.Add(go);
    }
}
