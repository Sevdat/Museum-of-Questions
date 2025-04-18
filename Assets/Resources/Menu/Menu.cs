using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    internal FolderPaths folderPaths;
    public GameObject terminalButton,assetTerminalButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        terminalButton.GetComponent<Button>().onClick.AddListener(() => onTerminalButtonClick());
        assetTerminalButton.GetComponent<Button>().onClick.AddListener(() => onAssetTerminalButtonClick());

    }

    public void onTerminalButtonClick(){
        folderPaths.terminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }
    public void onAssetTerminalButtonClick(){
        folderPaths.assetTerminalGameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
