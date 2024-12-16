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
    public RectTransform content;
    public GameObject button;
    public PathScript pathScript;
    public List<GameObject> buttons = new List<GameObject>();
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
        string[] files = Directory.GetFiles(pathScript.pathToString());
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
        pathScript.addButton(button.name);
        pathScript.folders.deleteButtons();
        deleteButtons();
        Process.Start(pathScript.pathToString());
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
