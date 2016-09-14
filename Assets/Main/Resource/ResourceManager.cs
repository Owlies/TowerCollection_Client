using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {

    private static ResourceManager m_instance;
    private ResourceManager() { }

    public static ResourceManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(ResourceManager)) as ResourceManager;
            return m_instance;
        }
    }

    private Dictionary<string, GameObject> loadedObjects;
    private Dictionary<string, Sprite> loadedSprites;
    private Dictionary<string, TextAsset> loadedTextAssets;

    private List<string> prefabLoadList;
    private List<string> textureLoadList;
    private List<string> textAssetLoadList;

    private int totalLoad, currentLoaded;

    public void AddPreLoadResource(RESOURCE_TYPE type, string path)
    {
        switch(type)
        {
            case RESOURCE_TYPE.RESOURCE_PREFAB:
                prefabLoadList.Add(path);
                break;
            case RESOURCE_TYPE.RESOURCE_TEXTURE:
                textureLoadList.Add(path);
                break;
            case RESOURCE_TYPE.RESOURCE_TEXTASSET:
                textAssetLoadList.Add(path);
                break;
        }
    }

    public IEnumerator StartPreLoad()
    {
        totalLoad = prefabLoadList.Count + textureLoadList.Count + textAssetLoadList.Count;

        for (int i = 0; i < prefabLoadList.Count; i++)
        {
            Load(prefabLoadList[i], RESOURCE_TYPE.RESOURCE_PREFAB);
            yield return 0;
        }
        for (int i = 0; i < textureLoadList.Count; i++)
        {
            Load(textureLoadList[i], RESOURCE_TYPE.RESOURCE_TEXTURE);
            yield return 0;
        }
        for (int i = 0; i < textAssetLoadList.Count; i++)
        {
            Load(textAssetLoadList[i], RESOURCE_TYPE.RESOURCE_TEXTASSET);
            yield return 0;
        }

        GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_PRELOAD_TOTAL_FINISH);

    }

    /// <summary>
    /// return null if nothing loaded;
    /// </summary>
    /// <param name="str"> path </param>
    /// <returns></returns>
    public GameObject GetResourceObject(string path)
    {
        if (!loadedObjects.ContainsKey(path))
        {
            GameObject obj = Resources.Load<GameObject>(path);
            if (obj == null)
            {
                GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED, path);
                return null;
            }
            loadedObjects.Add(path, obj);
        }

        //TODO: what if failed?

        return loadedObjects[path];
    }

    /// <summary>
    /// return null if nothing loaded;
    /// </summary>
    /// <param name="str"> path </param>
    /// <returns></returns>
    public Sprite GetResourceSprite(string path)
    {
        if (!loadedSprites.ContainsKey(path))
        {
            Sprite sprt = Resources.Load<Sprite>(path);
            if (sprt == null)
            {
                GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED, path);
                return null;
            }
            loadedSprites.Add(path, sprt);
        }

        //TODO: what if failed?

        return loadedSprites[path];
    }

    /// <summary>
    /// return null if nothing loaded;
    /// </summary>
    /// <param name="str"> path </param>
    /// <returns></returns>
    public TextAsset GetResourceTextAsset(string path)
    {
        if (!loadedTextAssets.ContainsKey(path))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            if (textAsset == null)
            {
                GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED, path);
                return null;
            }
            loadedTextAssets.Add(path, textAsset);
        }

        return loadedTextAssets[path];
    }

    private void Load(string path, RESOURCE_TYPE type)
    {
        Event evt = new Event(EVT_TYPE.EVT_TYPE_PRELOAD_PARTIAL_FINISH);

        switch (type)
        {
            case RESOURCE_TYPE.RESOURCE_PREFAB:
                GameObject obj = Resources.Load<GameObject>(path);
                if (obj == null)
                {
                    GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED,path);
                    return;
                }
                evt.evt_obj.Add(obj);
                if (!loadedObjects.ContainsKey(path))
                    loadedObjects[path] = obj;
                break;
            case RESOURCE_TYPE.RESOURCE_TEXTURE:
                Sprite sprite = Resources.Load<Sprite>(path);
                if (sprite == null)
                {
                    GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED,path);
                    return;
                }
                evt.evt_obj.Add(sprite);
                if (!loadedSprites.ContainsKey(path))
                    loadedSprites[path] = sprite;
                break;
            case RESOURCE_TYPE.RESOURCE_TEXTASSET:
                TextAsset text = Resources.Load<TextAsset>(path);
                if (text == null)
                {
                    GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED,path);
                    return;
                }
                evt.evt_obj.Add(text);
                if (!loadedTextAssets.ContainsKey(path))
                    loadedTextAssets[path] = text;
                break;
        }

        currentLoaded++;
        evt.evt_obj.Add((float)currentLoaded / totalLoad);

        GameManager.Instance.SendEvent(evt);
    }

	// Use this for initialization
	void Start () {

	}

    public void Init()
    {
        loadedObjects = new Dictionary<string, GameObject>(200);
        loadedSprites = new Dictionary<string, Sprite>(500);
        loadedTextAssets = new Dictionary<string, TextAsset>(200);
        prefabLoadList = new List<string>(200);
        textureLoadList = new List<string>(500);
        textAssetLoadList = new List<string>(200);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
