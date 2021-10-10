using UnityEngine;

public static class GetComponents
{   
    public static bool TryGetAComponent<T>(this GameObject target, out T component) where T : Component
    {
        component = target.GetComponent<T>();
        return component != null;
    }

    public static bool TryGetAComponent<T>(this Transform target, out T component) where T : Component
    {
        component = target.GetComponent<T>();
        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject target, out T component) where T : Component
    {
        component = target.GetComponentInChildren<T>();
        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this Transform target, out T component) where T : Component
    {
        component = target.GetComponentInChildren<T>();
        return component != null;
    }

    public static bool TryGetComponentsInChildren<T>(this GameObject target, out T[] components) where T : Component
    {
        components = target.GetComponentsInChildren<T>();
        return components != null;
    }

    public static bool TryGetComponentsInChildren<T>(this Transform target, out T[] component) where T : Component
    {
        component = target.GetComponentsInChildren<T>();
        return component != null;
    }
}