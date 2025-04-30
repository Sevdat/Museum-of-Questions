using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                StartCoroutine(saveMap());
            }
        } else if (Input.anyKeyDown){
            char pressedKey = GetPressedKey()[0];
            bool symbols = !char.IsLetterOrDigit(pressedKey) && !excludedChars.Contains(pressedKey);
            bool digitsAndNum = char.IsLetterOrDigit(pressedKey);
            if (symbols || digitsAndNum){
                inputBuffer += pressedKey;
                displayText.text = inputBuffer;
            }
        }
    }

    IEnumerator saveMap(){
        yield return main.exportImportGLTF.ExportGameObject(main.currentMap,Application.dataPath+$"/Resources/GeneratedMaps/{inputBuffer}");
        transform.gameObject.SetActive(false);
    }
    private string GetPressedKey(){
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
    void deleteOldFolder(string folderPath){
        Directory.Delete(folderPath, recursive: true);
    }
}
