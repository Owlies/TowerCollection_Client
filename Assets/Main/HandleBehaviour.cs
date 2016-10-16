using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;

public class HandleBehaviour : MonoBehaviour {

    public void start()
    {
        Debug.Assert(!OverridesUpdateMethod("Update"), "Do not use " + name + "() in class " + GetType() + "! Override HandleUpdate() instead!");
        Debug.Assert(!OverridesUpdateMethod("OnEnable"), "Do not use " + name + "() in class " + GetType() + "! Override HandleEnable() instead!");
        Debug.Assert(!OverridesUpdateMethod("OnDisable"), "Do not use " + name + "() in class " + GetType() + "! Override HandleDisable() instead!");
    }

    private bool OverridesUpdateMethod(string methodName)
    {
        return GetType() != typeof(HandleBehaviour) &&
               GetType().GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
    }

    private void OnEnable()
    {
        UpdateManager.Instance.RegisterUpdateFun(HandleUpdate);
        HandleEnable();
    }

    private void OnDisable()
    {
        UpdateManager.Instance.UnRegisterUpdateFun(HandleUpdate);
        HandleDisable();
    }

    protected virtual void HandleUpdate()
    {

    }

    protected virtual void HandleEnable()
    {

    }

    protected virtual void HandleDisable()
    {

    }
}
