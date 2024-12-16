using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFolderScript : MonoBehaviour
{
    public RectTransform content;
    public GameObject button;
    public PathScript pathScript;
    public Dictionary<GameObject,string> buttons = new Dictionary<GameObject,string>();
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
    public void specialFolderPaths(){
        Environment.SpecialFolder[] folderNames = 
            (Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder));
        foreach (Environment.SpecialFolder folderName in folderNames){

            string fullPath = Environment.GetFolderPath(folderName);
            if (fullPath.Length>0){
                string[] strArray = fullPath.Split("\\");
                string name = strArray[strArray.Length-1];
                GameObject gameObject = createItem(name);
                buttons[gameObject] = fullPath;
            }
        }
    }
    public void onClick(Button button){
        pathScript.resetPath(buttons[button.gameObject]);
        pathScript.folders.getFolders();
        pathScript.files.getFiles();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
