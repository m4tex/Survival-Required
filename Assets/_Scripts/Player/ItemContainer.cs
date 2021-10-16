using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public List<GameObject> items;
    public enum MaxItemSize { Small, Normal, Large, ExtraLarge }
    public MaxItemSize maxItemSize;

    public GameObject m1, m2;
    public Collider interCollider;

    private HashSet<Collider> colliders = new HashSet<Collider>();
    public HashSet<Collider> GetColliders() { return colliders; }


    public float GetWeight()
    {
        float weight = 0;
        foreach(GameObject item in items)
            weight += item.GetComponent<IStorable>().Weight;
        return weight;
    }

    public void CloseContainer()
    {
        m1.Toggle(false);
        m2.Toggle(true);

        foreach (Collider item in colliders)
        {
            if (item.GetComponent<IStorable>() != null) 
            {
                item.transform.SetParent(transform);
                items.Add(item.gameObject);
                item.gameObject.Toggle(false);
            }
        }
    }

    public void OpenContainer()
    {
        m1.Toggle(true);
        m2.Toggle(false);

        foreach (GameObject item in items)
        {
            item.Toggle(true);
            item.transform.parent = null;
        }
    }


    private void OnTriggerEnter(Collider other) =>
        colliders.Add(other); //hashset automatically handles duplicates

    private void OnTriggerExit(Collider other) =>
        colliders.Remove(other);
}