using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorable
{
    public float Weight { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
}