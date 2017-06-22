using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportMeshes : MonoBehaviour {
    private GameObject tempModel;
    private string newFolderPath = "/" + "LoadedModels";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists) {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName)) {
            //Directory.Delete(destDirName,true);
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files) {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs) {
            foreach (DirectoryInfo subdir in dirs) {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
    public void LoadInModels() {
        string path = EditorUtility.OpenFilePanel("choose file", Application.dataPath, "fbx");
        if (path == "") return;
        System.IO.DirectoryInfo dirInfo = System.IO.Directory.GetParent(path);
        DirectoryCopy(dirInfo.FullName, Application.dataPath + newFolderPath, false);
        AssetDatabase.Refresh();
        
        //tempModel = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets" + newFolderPath + "/testModel1.fbx", typeof(GameObject)));
        tempModel = Instantiate((GameObject)Resources.Load("Assets" + newFolderPath + "/testModel1.fbx", typeof(GameObject)));
        ObjectManager.singleton.model = tempModel;
    }
}
