using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    internal Main main;
    public GameObject terminalButton,assetTerminalButton,saveMap,playMode,editorMode;
    
    // Start is called before the first frame update
    void Start()
    {
        terminalButton.GetComponent<Button>().onClick.AddListener(() => onTerminalButtonClick());
        assetTerminalButton.GetComponent<Button>().onClick.AddListener(() => onAssetTerminalButtonClick());
        saveMap.GetComponent<Button>().onClick.AddListener(() => onSaveMapButtonClick());
        editorMode.GetComponent<Button>().onClick.AddListener(() => onEditorModeButtonClick());
        playMode.GetComponent<Button>().onClick.AddListener(() => onPlayModeButtonClick());
    }

    public void onTerminalButtonClick(){
        main.terminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }
    public void onAssetTerminalButtonClick(){
        main.assetTerminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
        main.assetTerminal.onMenuClick();
    }
    public void onSaveMapButtonClick(){
        main.textBoxTerminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    } 
    public void onEditorModeButtonClick(){
        main.firstPerson.enabled = true;
        main.thirdPerson.enabled = false;
        transform.gameObject.SetActive(false);
    } 
    public void onPlayModeButtonClick(){
        main.thirdPerson.enabled = true;
        main.firstPerson.enabled = false;
        transform.gameObject.SetActive(false);
    } 
}
