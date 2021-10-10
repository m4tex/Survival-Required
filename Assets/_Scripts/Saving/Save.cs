using System;
using UnityEngine;
[Serializable]
public class Save
{
    //Save info
    public bool isUsed = false;
    public string saveName;
    //Used to load the correct environment. Should be changed after introducing procedural generation of the map.
    public int mapIndex;

    public DateTime lastPlayed;
    public TimeSpan timeSpent;

    //Player position
    public float px;
    public float py;
    public float pz;
    //Player rotation
    public float rx;
    public float ry;
    //Player velocity
    public float vx;
    public float vy;
    public float vz;

    //Physical State
    public int health;
    public int temperature;
    public int nutrition;
    public int hydration;
    //Inventory inv;

    #region Help Methods
    public enum VType { playerPos, playerRot, playerVel };
    
    //Binary Formatter used for saving the important data can't accept Vector3's, these methods are converting the separate int's into Vector3's for further usage.
    public Vector3 GetVectorData(VType vd)
    {
        if (vd == VType.playerPos)
            return new Vector3(px, py, pz);
        else if (vd == VType.playerRot)
            return new Vector3(rx, ry, 0);
        else if (vd == VType.playerVel)
            return new Vector3(vx, vy, vz);
        else
            return Vector3.zero;
    }

    public void SetVectorData(Vector3 v3, VType vd)
    {
        if (vd == VType.playerPos)
        {
            px = v3.x;
            py = v3.y;
            pz = v3.z;
        }
        else if (vd == VType.playerRot)
        {
            rx = v3.x;
            ry = v3.y;
        }
        else if (vd == VType.playerVel)
        {
            vx = v3.x;
            vy = v3.y;
            vz = v3.z;
        }
    }
    #endregion
}