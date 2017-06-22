using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;

public class MeshImporter : MonoBehaviour {

    private GameObject tempModel;
    public bool finishedLoadingModels;
    public string subfolderModelsName;
    public List<GameObject> loadedModels = new List<GameObject>();

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadModelsFromResources() {
        Object[] meshes = Resources.LoadAll(subfolderModelsName,typeof(GameObject)) as Object[];
        foreach(GameObject mesh in meshes) {
            loadedModels.Add((GameObject)Resources.Load(subfolderModelsName + "/" + mesh.name) as GameObject);
        }
        finishedLoadingModels = true;
        tempModel = Instantiate(loadedModels[0]);
        ObjectManager.singleton.model = tempModel;
    }
    private string GetFileName(string filePath) {
        char[] delimiters = new char[] { '/', '.' };
        string[] filePathParts = filePath.Split(delimiters);
        string fileName = filePathParts.GetValue(filePathParts.Length - 2).ToString();
        return fileName;
    }
#if UNITY_EDITOR
    //Non Build Code
    public void LoadInModels() {
        string newFolderPath = "/LoadedModels/";
        string path = EditorUtility.OpenFilePanel("choose file", Application.dataPath, "fbx");
        if (path == "") return;
        System.IO.DirectoryInfo dirInfo = System.IO.Directory.GetParent(path);
        DirectoryCopy(dirInfo.FullName, Application.dataPath + newFolderPath, false);
        AssetDatabase.Refresh();
        tempModel = Instantiate((GameObject)Resources.Load("Assets" + newFolderPath + "/testModel1.fbx", typeof(GameObject)));
        ObjectManager.singleton.model = tempModel;
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
#endif
}
