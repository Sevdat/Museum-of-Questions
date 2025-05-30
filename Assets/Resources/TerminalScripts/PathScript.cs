using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathScript : MonoBehaviour
{
    public GameObject content;
    public GameObject button;
    public TerminalScript terminalScript;
    internal List<GameObject> buttons = new List<GameObject>();
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
    public string pathToString(){
        string str = "";
        foreach(GameObject gameObject in buttons){
            str += (str.Length >0)? "\\"+gameObject.name:gameObject.name;
        }
        return str;
    }
    public void addButton(string str){
        buttons.Add(createItem(str));
    }
    public void deleteButtons(){
        for(int i = 0;i<buttons.Count;i++){
            Destroy(buttons[i]);
        }   
        buttons = new List<GameObject>();
    }
    public void resetPath(string str){
        deleteButtons();
        string[] strArray = str.Split("\\");
        for (int i = 0;i<strArray.Length;i++){
            buttons.Add(createItem(strArray[i]));
        }
    }
    public void onClick(Button button){
        bool check = false;
        for (int i = 0;i<buttons.Count;i++) {
            if (check) {
                Destroy(buttons[i]);
                buttons.RemoveAt(i);
                i--;
                }
            if (buttons[i] == button.gameObject) check = true;
        }
        terminalScript.folders.getFolders();
        terminalScript.files.getFiles();
    }
}
