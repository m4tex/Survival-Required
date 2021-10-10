using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class Saving : MonoBehaviour
{

    public int saveID = 0;
    public Save[] loadedSaves;
    public static Saving instance;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
        if (!TryReadSaveFiles(out loadedSaves))
            loadedSaves = new Save[8];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            WriteSaveFile(UpdateSaveFile());
        if (Input.GetKeyDown(KeyCode.M))
            LoadSave(saveID);
    }

    //Writes all the save data to memory.
    public void WriteSaveFile(Save[] sf)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.dataPath + "/saveData.dat");
        bf.Serialize(fileStream, sf);
        fileStream.Close();
    }

    //Tries to read the data from memory and returns a boolean. Can also output the save file if the read was successful.
    public bool TryReadSaveFiles(out Save[] saves)
    {
        if (File.Exists(Application.dataPath + "/saveData.dat"))
        {
            print("save data found.");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.dataPath + "/saveData.dat", FileMode.Open);
            Save[] loadedSaves = bf.Deserialize(fs) as Save[];

            fs.Close();

            saves = loadedSaves;
            return true;
        }
        saves = null;
        return false;
    }

    //Updates the variables in the current save slot and returns the Saves array.
    public Save[] UpdateSaveFile()
    {
        loadedSaves[saveID].isUsed = true;
        loadedSaves[saveID].mapIndex = SceneManager.GetActiveScene().buildIndex;

        Transform player = InGameManager.player.transform;
        loadedSaves[saveID].SetVectorData(player.GetComponent<Rigidbody>().velocity, Save.VType.playerVel);
        loadedSaves[saveID].SetVectorData(player.transform.position, Save.VType.playerPos);
        loadedSaves[saveID].SetVectorData(new Vector3(MouseLook.instance.transform.localEulerAngles.x, player.transform.eulerAngles.y, 0), Save.VType.playerRot);

        return loadedSaves;
    }

    //Apllies the read data (2 methods below)
    public void LoadSave(int slotID)
    {
        saveID = slotID;
        SceneManager.LoadScene(loadedSaves[slotID].mapIndex);
        SceneManager.sceneLoaded += ApplySave;
    }

    public void ApplySave(Scene scene, LoadSceneMode mode)
    {
        InGameManager.player.transform.position = loadedSaves[saveID].GetVectorData(Save.VType.playerPos);
        InGameManager.player.transform.eulerAngles = new Vector3(0, loadedSaves[saveID].GetVectorData(Save.VType.playerRot).y, 0);
        InGameManager.player.GetComponent<Rigidbody>().velocity = loadedSaves[saveID].GetVectorData(Save.VType.playerVel);
        Camera.main.GetComponent<MouseLook>().SetXRotation(loadedSaves[saveID].GetVectorData(Save.VType.playerRot).x);
    }

    public void CreateNewSave(Save initalData)
    {
        //Finds the next free save slot
        for (int i = 0; i < loadedSaves.Length-1; i++)
        {
            if (!loadedSaves[i].isUsed)
            {
                saveID = i;
                break;
            }
        }
        loadedSaves[saveID] = initalData;
        SceneManager.LoadScene(initalData.mapIndex);
    }
}