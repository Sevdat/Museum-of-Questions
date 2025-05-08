using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TextBoxTerminal : MonoBehaviour
{
    internal Main main;
    public TMP_Text displayText;
    private string inputBuffer = "";
    HashSet<char> excludedChars = new HashSet<char>();
    void Awake(){
        excludedChars.UnionWith(Path.GetInvalidPathChars());
        excludedChars.Add('/');
        excludedChars.Add('\\');
    }
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Backspace)){
            if (inputBuffer.Length > 0){
            inputBuffer = inputBuffer.Remove(inputBuffer.Length-1);
            displayText.text = inputBuffer;
            }
        }else if (Input.GetKeyDown(KeyCode.Return)){
            if (inputBuffer.Length > 0 && inputBuffer.ToString().IndexOfAny(Path.GetInvalidPathChars()) == -1){
                save();
            }
        } else if (Input.anyKeyDown){
            char pressedKey = getPressedKey()[0];
            bool symbols = !char.IsLetterOrDigit(pressedKey) && !excludedChars.Contains(pressedKey);
            bool digitsAndNum = char.IsLetterOrDigit(pressedKey);
            if (symbols || digitsAndNum){
                inputBuffer += pressedKey;
                displayText.text = inputBuffer;
            }
        }
    }
    internal void save(){
        string path = main.orginizePaths.mapPath;
        string[] dir = Directory.GetDirectories(path);
        bool exists = false;
        foreach (string str in dir){
            if (Path.GetFileName(str) == inputBuffer) {
                exists = true;
                break;
            }
        }
        if (!exists) StartCoroutine(saveMap(path+$"/{inputBuffer}/{inputBuffer}.gltf"));
        else {
            string str = path+$"/{inputBuffer}";
            Directory.Delete(str, recursive: true);
            StartCoroutine(saveMap(str + $"/{inputBuffer}.gltf"));
        }
        main.orginizePaths.saveData();
    }
    internal IEnumerator saveMap(string mapPath, bool setKey = true){
        if (setKey) main.orginizePaths.setKey(mapPath);
        main.currentMap.transform.name = inputBuffer;
        yield return main.exportImportGLTF.ExportGameObject(main.currentMap,Application.dataPath+$"/Resources/GeneratedMaps/{inputBuffer}");
        transform.gameObject.SetActive(false);
    }
    private string getPressedKey(){
        // First check for character input
        if (!string.IsNullOrEmpty(Input.inputString)){
            return Input.inputString;
        }

        // Check all KeyCode values
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))){
            if (Input.GetKeyDown(keyCode)){
                // Format special keys
                return keyCode.ToString();
            }
        }

        return "";
    }
}
