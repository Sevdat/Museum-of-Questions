using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : MonoBehaviour
{
    Terminal terminal;
    public class Terminal{
        GameObject terminal;
        PathScript path;
        SpecialFolderScript specialFolder;
        FolderScript folders;
        FileScript files;
        public Terminal(){
            terminal = Resources.Load("Terminal") as GameObject;
            terminal = Instantiate(terminal);
            path = getTerminalWindow(0).GetComponent<PathScript>();
            specialFolder = getTerminalWindow(1).GetComponent<SpecialFolderScript>();
            path.specialFolder = specialFolder;
            specialFolder.pathScript = path;

            folders = getTerminalWindow(2).GetComponent<FolderScript>();
            path.folders = folders;
            folders.pathScript = path;

            files = getTerminalWindow(3).GetComponent<FileScript>();
            path.files = files;
            files.pathScript = path;

            specialFolder.specialFolderPaths();
        }
        Transform getTerminalWindow(int index){
            return terminal.transform.GetChild(index).GetChild(0).GetChild(0);
        }
        public void destroy(){
            Destroy(terminal);
            terminal = null;
        }
    }
    void Start() {
    }
    bool exist = false;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            if (exist == false){
                terminal = new Terminal();
                exist = true;
            } else {
                terminal.destroy();
                exist = false;
            }
        }
    }

}
