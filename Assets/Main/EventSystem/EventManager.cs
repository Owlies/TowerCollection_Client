using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Handler(Event evt);

public class EventManager : MonoBehaviour {

    
    private static EventManager m_instance;
    private EventManager() { }

    public static EventManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
            return m_instance;
        }
    }

    private Queue m_eventsQueue;

    private Dictionary<EVT_TYPE, Handler> m_callBacks;

    public void BindEvent(EVT_TYPE t, Handler hdr)
    {
        m_callBacks[t] = hdr;
    }

    public void UnbindEvent(EVT_TYPE t)
    {
        if (m_callBacks.ContainsKey(t))
            m_callBacks.Remove(t);
    }

    public void SendEvent(Event evt, bool handleNow = false)
    {
        if (handleNow)
            HandleEvent(evt);
        else
            m_eventsQueue.Enqueue(evt);
    }

    public void SendEvent(EVT_TYPE t, object obj, bool handleNow = false)
    {
        Event evt = new Event(t, obj);
        if (handleNow)
            HandleEvent(evt);
        else
            m_eventsQueue.Enqueue(evt);
    }

    public void SendEventWithList(EVT_TYPE t, ArrayList list, bool handleNow = false)
    {
        Event evt = new Event(t, list);
        if (handleNow)
            HandleEvent(evt);
        else
            m_eventsQueue.Enqueue(evt);
    }

	// Use this for initialization
	void Start () {

	}

    public void Init()
    {
        m_eventsQueue = new Queue((int)EVT_TYPE.EVT_TYPE_MAX);
        m_callBacks = new Dictionary<EVT_TYPE, Handler>((int)EVT_TYPE.EVT_TYPE_MAX);

        BindEvent(EVT_TYPE.EVT_TYPE_DEFAULT, new Handler(DefaultEventHandler.Handle));

        BindEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME, new Handler(GameLoopEventHandler.EnterGame));
        BindEvent(EVT_TYPE.EVT_TYPE_UNIT_DIE, new Handler(GameLoopEventHandler.UnitDie));
        BindEvent(EVT_TYPE.EVT_TYPE_CHANGE_EC, new Handler(GameLoopEventHandler.ChangeElementCrystal));

		BindEvent(EVT_TYPE.EVT_TYPE_RELEASE_SKILL, new Handler(GameLoopEventHandler.ReleaseSkill));
        

        BindEvent(EVT_TYPE.EVT_TYPE_LOAD_FAILED, new Handler(LoadEventHandler.LoadFailed));

        //temp
        //BindEvent(EVT_TYPE.EVT_TYPE_PRELOAD_TOTAL_FINISH, new Handler(GameLoopEventHandler.EnterGame));
    }

    private void HandleEvent(Event evt)
    {
        if (evt.type < EVT_TYPE.EVT_TYPE_MAX && m_callBacks.ContainsKey(evt.type))
        {
            Handler hdr = m_callBacks[evt.type] as Handler;
            if (hdr != null)
                hdr(evt);
        }
        else
        {
            Debug.LogWarning("Wrong Event Type!");
            Debug.LogWarning(evt.type);
        }
    }

	// Update is called once per frame
	void Update () {
	    while(m_eventsQueue.Count != 0)
        {
            Event evt = m_eventsQueue.Dequeue() as Event;
            HandleEvent(evt);
        }
	}
}
