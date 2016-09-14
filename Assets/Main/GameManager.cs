using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private static GameManager m_instance;
    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            return m_instance;
        }
    }

    public GameObject GetResourceObject(string path)
    {
        return ResourceManager.Instance.GetResourceObject(path);
    }

    public TextAsset GetResourceTextAsset(string path)
    {
        return ResourceManager.Instance.GetResourceTextAsset(path);
    }

    public Sprite GetResourceSprite(string path)
    {
        return ResourceManager.Instance.GetResourceSprite(path);
    }

    /// <summary>
    /// CAUTION: each event can only bind one callback function!!!
    /// </summary>
    /// <param name="t">event type</param>
    /// <param name="hdr">call back(handler)</param>
    public void BindEvent(EVT_TYPE t, Handler hdr)
    {
        EventManager.Instance.BindEvent(t, hdr);
    }

    public void UnbindEvent(EVT_TYPE t)
    {
        EventManager.Instance.UnbindEvent(t);
    }

    public void SendEvent(EVT_TYPE t, object obj = null, bool handleNow = false)
    {
        EventManager.Instance.SendEvent(t, obj, handleNow);
    }

    public void SendEvent(EVT_TYPE t, ArrayList list, bool handleNow = false)
    {
        EventManager.Instance.SendEvent(t, list, handleNow);
    }

    public void SendEvent(Event evt, bool handleNow = false)
    {
        EventManager.Instance.SendEvent(evt, handleNow);
    }

    void startPreLoad()
    {
        //basic prefabs
        ResourceManager.Instance.AddPreLoadResource(RESOURCE_TYPE.RESOURCE_PREFAB, "Model/Prefabs/Monster");
        ResourceManager.Instance.AddPreLoadResource(RESOURCE_TYPE.RESOURCE_PREFAB, "Model/Prefabs/Tower");
        ResourceManager.Instance.AddPreLoadResource(RESOURCE_TYPE.RESOURCE_PREFAB, "Model/Prefabs/CrystalUI");

        StartCoroutine(ResourceManager.Instance.StartPreLoad());
    }

	// Use this for initialization
    void Start()
    {
        ResourceManager.Instance.Init();
        EventManager.Instance.Init();
        // open loading screen
        UIManager.Instance.PreInit();

        // pre load everying that we may need
        startPreLoad();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
