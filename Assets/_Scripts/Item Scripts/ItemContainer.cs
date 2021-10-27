using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour, IInteractable, IStorable, IOutlinable
{
    public List<GameObject> items;
    public enum MaxItemSize { Small, Normal, Large, ExtraLarge }
    public MaxItemSize maxItemSize;

    public GameObject m1, m2;
    private Rigidbody rb;

    private HashSet<Collider> colliders = new HashSet<Collider>();

    //Interface implementations
    public Dictionary<string, IInteractable.Interaction> interactions { get; set; } = new Dictionary<string, IInteractable.Interaction>();
    public float Weight { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public Outline OutlineComp { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        OutlineComp = gameObject.AddComponent(typeof(Outline)) as Outline;
        OutlineComp.OutlineMode = Outline.Mode.OutlineHidden;
        OutlineComp.OutlineWidth = 4;

        interactions.Add("Close Container", CloseContainer);
    }

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
        rb.isKinematic = false;
        

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
        rb.isKinematic = true;

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