using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDebug : MonoBehaviour
{
    //Because static fields can't be serialized, this method keeps track of it and let's you see its content in inspector.
    public Save[] loadedSaves;
    void Update()
    {
        loadedSaves = Saving.loadedSaves;
    }
}
