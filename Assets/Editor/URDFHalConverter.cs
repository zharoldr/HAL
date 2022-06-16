using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class URDFHalConverter : EditorWindow {
    public GameObject URDF_Object = null;

    private GameObject root = null;

    [MenuItem("Window/URDF-to-HAL")]
    public static void ShowWindow() {
        EditorWindow.GetWindow<URDFHalConverter>("URDF to HAL Converter");
    }

    void OnGUI() {
        GUILayout.Label("The URDF importer creates a GameObject that is difficult for me to control.\nThis tool converts the imported URDF into a prefab.");
        URDF_Object = (GameObject)EditorGUILayout.ObjectField("Imported URDF Tree", URDF_Object, typeof(GameObject), true);

        if (GUILayout.Button("Convert to Prefab for HAL")) {
            root = new GameObject("HAL-Robot");
            make_prefab(URDF_Object);

            if (!Directory.Exists("Assets/Prefabs")) {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
            }
            string localPath = "Assets/Prefabs/"+root.name+".prefab";

            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            bool success;
            PrefabUtility.SaveAsPrefabAssetAndConnect(root, localPath, InteractionMode.UserAction, out success);
        }
    }

    private void make_prefab(GameObject obj) {
        foreach(Transform child_transform in obj.transform) {

            if (child_transform.gameObject.name == "Visuals" && child_transform.childCount == 1) {
                Transform mesh       = child_transform.GetChild(0).GetChild(0);
                Transform mesh_trans = child_transform.GetChild(0);

                GameObject mesh_obj = (GameObject)Object.Instantiate(mesh.gameObject, root.transform);
                
                mesh_obj.transform.position = mesh_trans.position;
                mesh_obj.transform.localRotation = mesh_trans.localRotation;
            }

            make_prefab(child_transform.gameObject);
        }
    }
}
