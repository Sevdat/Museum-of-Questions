using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : MonoBehaviour
{
    internal Main folderPaths;
    public PathScript path;
    public SpecialFolderScript specialFolder;
    public FolderScript folders;
    public FileScript files;
    
    public void Start()
    {
        specialFolder.specialFolderPaths();
    }
    public void Update(){

    }

}
