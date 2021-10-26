using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    //Holds the player game object.
    public GameObject _player;
    public static GameObject player;

    //Used for the ESC Menu.
    public GameObject escMenu;
    //Post processing effects (field of view trick used for blur)
    public VolumeProfile postProcessing;
    private DepthOfField blurDepth;
    private void Awake()
    {
        player = _player;
        postProcessing.TryGet(out blurDepth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick_TogglePause();
    }

    public void OnClick_TogglePause()
    {
        escMenu.Toggle();
        blurDepth.active.Toggle();
        Time.timeScale = Time.timeScale.ToggleBetween(0, 1);
        PlayerMovement.instance.movementLock.Toggle();
        MouseLook.instance.cameraLock.Toggle();
    }

    public void OnClick_LoadLastSave()
    {
        blurDepth.active.Toggle();
        Saving.LoadSave(Saving.saveID);
    }

    public void OnClick_SaveProgress()
    {
        Saving.WriteSaveFile(Saving.UpdateSaveFile());
        OnClick_TogglePause();
    }
    public void OnClick_SaveAndLeave()
    {
        Saving.WriteSaveFile(Saving.UpdateSaveFile());
        SceneManager.LoadScene(0);
    }
}