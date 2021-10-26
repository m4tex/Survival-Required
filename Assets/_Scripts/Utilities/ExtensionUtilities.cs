using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionUtilities

{
    public static void Toggle(this GameObject target)
    {
        if (target.activeInHierarchy)
            target.SetActive(false);
        else
            target.SetActive(true);
    }
    public static void Toggle(this ref bool target)
    {
        target = !target;
    }

    public static void Toggle(this GameObject target, bool toState)
    {
        target.SetActive(toState);
    }

    public static void ToggleOnly(this GameObject target, in GameObject[] collection)
    {
        foreach (GameObject item in collection)
        {
            item.Toggle(false);
        }
        target.Toggle(true);
    }

    public static IEnumerable ToggleForSeconds(this GameObject target, float time)
    {
        target.Toggle();

        yield return new WaitForSeconds(time);

        target.Toggle();
    }
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

    public static T[] GetComponentsInArray<T>(this GameObject[] array) where T: Component
    {
        T[] components = new T[array.Length];
        for (int i = 0; i < array.Length-1; i++)
        {
            components[i] = array[i].GetComponent<T>();
        }
        return components;
    }

    /// <summary>
    /// Tries to find an index that mathes the given value. If it fails, it returns a 0 in the out parameter, be careful.
    /// </summary>
    /// <returns>Whether it was successfull finding the index or not.</returns>
    public static bool TryFindIndex<T>(this T[] array, T value, out int index)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            if (EqualityComparer<T>.Default.Equals(array[i], value)) //think of this as of if(array[i] == value)
            {
                index = i;
                return true;
            }
        }
        index = 0;
        return false;
    }

    public static void DestroyChildren(this Transform target)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            Object.Destroy(target.GetChild(i));
        }
    }

    public static void DestroyChildren(this Transform target, int excludingIndex)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            if(i != excludingIndex)
                Object.Destroy(target.GetChild(i));
        }
    }
}