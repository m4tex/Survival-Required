using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBox : Pickable, IStorable, IOutlinable
{
    public float Weight { get; set; } = 2;
    public string ItemName { get; set; } = "Test Box";
    public string ItemDescription { get; set; } = "This is a box O.O";
    public Outline OutlineComp { get; set; }

    private void Awake()
    {
        OutlineComp = gameObject.AddComponent<Outline>();
        OutlineComp.OutlineWidth = 4;
        OutlineComp.OutlineMode = Outline.Mode.OutlineHidden;
    }
}
