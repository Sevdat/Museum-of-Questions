using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FolderScript : MonoBehaviour
{
    public GameObject content;
    public GameObject button;
    public TerminalScript terminalScript;
    internal List<GameObject> buttons = new List<GameObject>();

    internal bool spawnFolder = false;
    public GameObject folderSpawner;
    Button folderSpawnerButton;
    ColorBlock colorBlock;


    public GameObject createItem(string strName){
        GameObject gameObject;
        gameObject = Instantiate(button, Vector3.zero, Quaternion.identity);
        gameObject.transform.SetParent(content.transform);
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = new Vector3();
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3());
        gameObject.name = strName;
        Button clone = gameObject.GetComponent<Button>();
        clone.onClick.AddListener(() => onClick(clone));
        clone.GetComponentInChildren<TMP_Text>().text = strName;

        return gameObject;
    }
    public void getFolders(){
        string[] files = Directory.GetDirectories(terminalScript.path.pathToString());
        deleteButtons();
        foreach(string file in files){
            string[] strArray = file.Split("\\");
            string name = strArray[strArray.Length-1];
            buttons.Add(createItem(name));
        }
    }
    public void deleteButtons(){
        for(int i = 0;i<buttons.Count;i++){
            Destroy(buttons[i]);
        }
        buttons = new List<GameObject>();
    }
    public void onClick(Button button){
        if (spawnFolder){
            terminalScript.folderPaths.editPrefab.createPortal(terminalScript.path.pathToString() + $"\\{button.name}");
        } else {
            terminalScript.path.addButton(button.name);
            getFolders();
            terminalScript.files.getFiles();
        }
    }

    void Start()
    {
        folderSpawnerButton = folderSpawner.GetComponent<Button>();
        folderSpawnerButton.onClick.AddListener(() => onClick());
        colorBlock = folderSpawnerButton.colors;
    }
    public void onClick(){
        toggleHighlight();
        print($"Folder: {spawnFolder}");
    }
    void toggleHighlight()
    {
        spawnFolder = !spawnFolder;
        ColorBlock newColors = colorBlock;
        // Change the normal color to keep the highlight appearance
        newColors.normalColor = spawnFolder ? newColors.selectedColor : colorBlock.normalColor;
        newColors.selectedColor = spawnFolder ? newColors.selectedColor : colorBlock.normalColor;
        newColors.highlightedColor = spawnFolder ? newColors.selectedColor : colorBlock.normalColor;
        folderSpawnerButton.colors = newColors;
    }
}
