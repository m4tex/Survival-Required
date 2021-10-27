using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Pickable, IInteractable, IStorable, IOutlinable
{
    public List<GameObject> items;
    public enum MaxItemSize { Small, Normal, Large, ExtraLarge }
    public MaxItemSize maxItemSize;

    public GameObject m1, m2;
    private Rigidbody rigidB;

    private HashSet<Collider> colliders = new HashSet<Collider>();

    //Interface implementations
    public Dictionary<string, IInteractable.Interaction> interactions { get; set; } = new Dictionary<string, IInteractable.Interaction>();
    public float Weight { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public Outline OutlineComp { get; set; }

    private void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        OutlineComp = gameObject.AddComponent(typeof(Outline)) as Outline;
        OutlineComp.OutlineMode = Outline.Mode.OutlineHidden;
        OutlineComp.OutlineWidth = 4;

        interactions.Add("Close Container", CloseContainer);

        base.Start();
    }

    public float GetWeight()
    {
        float weight = 1;
        foreach(GameObject item in items)
            weight += item.GetComponent<IStorable>().Weight;
        return weight;
    }

    public void CloseContainer()
    {
        m1.Toggle(false);
        m2.Toggle(true);
        rigidB.isKinematic = false;
        canHold = true;

        foreach (Collider item in colliders)
        {
            if (item.GetComponent<IStorable>() != null) 
            {
                item.transform.SetParent(transform);
                items.Add(item.gameObject);
                item.gameObject.Toggle(false);
            }
        }
        rigidB.mass = GetWeight();

        interactions.Remove("Close Container");
        interactions.Add("Open Container", OpenContainer);
    }

    public void OpenContainer()
    {
        m1.Toggle(true);
        m2.Toggle(false);
        rigidB.isKinematic = true;
        canHold = false;

        foreach (GameObject item in items)
        {
            item.Toggle(true);
            item.transform.parent = null;
        }
        items.Clear();

        interactions.Remove("Open Container");
        interactions.Add("Close Container", CloseContainer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != this)
            colliders.Add(other); //hashset automatically handles duplicates
    }

    private void OnTriggerExit(Collider other) =>
        colliders.Remove(other);
}