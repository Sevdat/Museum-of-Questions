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
    public GameObject terminalButton,assetTerminalButton,textBoxTerminalButton;
    
    // Start is called before the first frame update
    void Start()
    {
        terminalButton.GetComponent<Button>().onClick.AddListener(() => onTerminalButtonClick());
        assetTerminalButton.GetComponent<Button>().onClick.AddListener(() => onAssetTerminalButtonClick());
        textBoxTerminalButton.GetComponent<Button>().onClick.AddListener(() => onTextBoxTerminalButtonClick());
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
    public void onTextBoxTerminalButtonClick(){
        main.textBoxTerminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    } 
}
