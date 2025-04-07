using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : MonoBehaviour
{
    public PathScript path;
    public SpecialFolderScript specialFolder;
    public FolderScript folders;
    public FileScript files;
    bool exist = false;
    public void Start()
    {
        specialFolder.specialFolderPaths();
    }
    public void Update(){

    }

}
