using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

class LongPath: MonoBehaviour
{
    void Start(){
        // Example of a very long path (>260 chars)
        string longBasePath = @"C:\temp\";
        string longSubDir = Path.Combine(
            new string('x', 100), 
            new string('x', 100),
            new string('x', 100),
            new string('x', 100),
            new string('x', 100)
        );
        string longPath = Path.Combine(longBasePath, longSubDir, "file.txt");


        print($"Path length: {longPath.Length} chars");
        print(longPath);

        // ===== 1. Create a Long Directory =====
        CreateLongDirectory(longPath);
        
        // ===== 2. Write to a File in Long Path =====
        WriteToLongFile(longPath, "Hello, long path world!");
        
        // ===== 3. Read from a File in Long Path =====
        string content = ReadFromLongFile(longPath);
        print($"File content: {content}");
        
        // ===== 4. Delete Long Path =====
        DeleteLongPath(longPath);
    }

    // ===== Helper Methods =====
    static string ConvertToExtendedPath(string path){
        // Prepend `\\?\` to enable long path support (if not already present)
        if (!path.StartsWith(@"\\?\")){
            if (path.StartsWith(@"\\")) // Network path (e.g., \\server\share)
                path = @"\\?\UNC\" + path.Substring(2);
            else // Local path (e.g., C:\...)
                path = @"\\?\" + path;
        }
        return path;
    }

    static void CreateLongDirectory(string path){
        string extendedPath = ConvertToExtendedPath(Path.GetDirectoryName(path));
        
        try{
            Directory.CreateDirectory(extendedPath);
            print($"Created directory: {extendedPath}");
        }
        catch (Exception ex){
            print($"Failed to create directory: {ex.Message}");
        }
    }

    static void WriteToLongFile(string path, string content){
        string extendedPath = ConvertToExtendedPath(path);
        
        try{
            File.WriteAllText(extendedPath, content);
            print($"Written to file: {extendedPath}");
        }
        catch (Exception ex){
            print($"Failed to write file: {ex.Message}");
        }
    }

    static string ReadFromLongFile(string path){
        string extendedPath = ConvertToExtendedPath(path);
        
        try{
            return File.ReadAllText(extendedPath);
        }
        catch (Exception ex){
            print($"Failed to read file: {ex.Message}");
            return null;
        }
    }

    static void DeleteLongPath(string path){
        string extendedPath = ConvertToExtendedPath(path);
        
        try {
            if (File.Exists(extendedPath))
                File.Delete(extendedPath);
            
            string dirPath = Path.GetDirectoryName(extendedPath);
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, recursive: true);
            
            print($"Deleted: {extendedPath}");
        }
        catch (Exception ex){
            print($"Failed to delete: {ex.Message}");
        }
    }
}