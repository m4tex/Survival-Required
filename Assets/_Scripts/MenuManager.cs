using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject newGameMenu;
    public GameObject playMenu;
    public GameObject menuButtons;

    public Transform savesScrollViewContent;
    public GameObject saveSlotItem;
    //Save creation variables
    [Header("Save Creation")]
    [SerializeField]
    public Toggle[] mapTogglesOrdered = new Toggle[1];
    public ToggleGroup mapToggle;
    public TMP_InputField saveNameInput;
    public string[] randomSaveNames;

    public void OnClick_Play()
    {
        playMenu.Toggle(true);
        DisplaySaves();
    }

    public void OnClick_NewSave()
    {
        playMenu.Toggle(false);
        newGameMenu.Toggle(true);
    }

    public void OnClick_NewGame(int levelID) =>
        SceneManager.LoadScene(levelID);

    public void OnClick_Quit() =>
        Application.Quit();

    //Displays save slots in menu
    public void DisplaySaves()
    {
        Save[] saveFiles;
        if (Saving.TryReadSaveFiles(out saveFiles))
        {
            foreach (Save save in saveFiles)
            {
                if (save.isUsed)
                {
                    GameObject slotDis = Instantiate(saveSlotItem, savesScrollViewContent);
                    slotDis.GetComponentInChildren<TMP_Text>().text = save.saveName;
                    int index;
                    if (saveFiles.TryFindIndex(save, out index))
                        slotDis.GetComponent<Button>().onClick.AddListener(delegate { Saving.LoadSave(index); });
                    else
                        Debug.LogError("Couldn't find the index of the save slot");
                }
            }
        }
    }

    public void OnClick_FillRandomName()
    {
        saveNameInput.text = randomSaveNames[Random.Range(0, randomSaveNames.Length)];
    }

    public void OnClick_CreateSave()
    {
        if (saveNameInput.text == "")
            OnClick_FillRandomName();
        Save iniSav = new Save();
        iniSav.saveName = saveNameInput.text;
        mapTogglesOrdered.TryFindIndex(mapToggle.GetFirstActiveToggle(), out iniSav.mapIndex);
        iniSav.mapIndex++;
        iniSav.isUsed = true;
        Saving.instance.CreateNewSave(iniSav);
    }
}