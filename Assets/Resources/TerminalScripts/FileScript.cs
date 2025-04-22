using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class FileScript : MonoBehaviour
{ 
    public GameObject content;
    public GameObject button;
    public TerminalScript terminalScript;
    internal List<GameObject> buttons = new List<GameObject>();

    internal bool spawnFile = false;
    public GameObject fileSpawner;
    Button fileSpawnerButton;
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
    public void getFiles(){
        string[] files = Directory.GetFiles(terminalScript.path.pathToString());
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
        if (!spawnFile) 
            Process.Start(terminalScript.path.pathToString() + $"/{button.name}"); 
        else
            terminalScript.folderPaths.editPrefab.createIcon(terminalScript.path.pathToString() + $"\\{button.name}");
    }
    public void onClick(){
        toggleHighlight();
        print($"File: {spawnFile}");
    }
    // Start is called before the first frame update
    void Start()
    {
        fileSpawnerButton = fileSpawner.GetComponent<Button>();
        fileSpawnerButton.onClick.AddListener(() => onClick());
        colorBlock = fileSpawnerButton.colors;
    }

    void toggleHighlight(){
        spawnFile = !spawnFile;
        ColorBlock newColors = colorBlock;
        // Change the normal color to keep the highlight appearance
        newColors.normalColor = spawnFile ? newColors.selectedColor : colorBlock.normalColor;
        newColors.selectedColor = spawnFile ? newColors.selectedColor : colorBlock.normalColor;
        newColors.highlightedColor = spawnFile ? newColors.selectedColor : colorBlock.normalColor;
        fileSpawnerButton.colors = newColors;
    }
}
