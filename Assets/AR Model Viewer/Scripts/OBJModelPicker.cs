using System;
using System.Collections;
using System.Collections.Generic;
using AsImpL;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

/// <summary>
/// Example to combine NativeFilePicker with objImporter.
/// </summary>
public class OBJModelPicker : MonoBehaviour
{
    private NativeFilePicker.FilePickedCallback _callback;

    private NativeFilePicker.Permission _permission;

    protected ImportOptions importOptions = new ImportOptions();
    protected ObjectImporter objImporter;
    
    [Tooltip("Null for no parent object for spawned objects.")]
    [SerializeField] private Transform SpawnParent;

    [SerializeField] private float scaleOnImport = 0.1f;
    private void Awake()
    {
        objImporter = GetComponent<ObjectImporter>();
    }

    private void OnEnable()
    {
        objImporter.ImportedModel += AfterModelImportedAndCreated; 
    }

    private void OnDisable()
    {
        objImporter.ImportedModel -= AfterModelImportedAndCreated;
    }

    void Start()
    {
        var fbxFileType = NativeFilePicker.ConvertExtensionToFileType("fbx");//unused, was testing things

        _permission = NativeFilePicker.CheckPermission();
        if (this._permission == NativeFilePicker.Permission.ShouldAsk)
        {
            NativeFilePicker.RequestPermission();
        }
    }
    public void SelectModel()
    {
        Debug.Log("select Model");
        // Pick a PDF file
        _permission = NativeFilePicker.PickFile(OnModelPicked, new string[] { "application/octet-stream","text/plain","application/object" });//obj types.

        Debug.Log("Permission result: " + _permission);
    }

    private void OnModelPicked(string path)
    {
        if (path == null)
        {
            Debug.Log("Cancelled");
            //cancelled
            return;
        }
        Debug.Log($"Importing... {path}");
        //
        objImporter.ImportModelAsync("Imported Object", path, null, importOptions);
        //we are subscribed to the callback when this completes. so next step is AfterModelImportedAndCreated.
    }

    private void AfterModelImportedAndCreated(GameObject createdModel, string path)
    {
        createdModel.transform.SetParent(SpawnParent);
        createdModel.transform.localScale *= scaleOnImport;//scale down
        
        //wrap a big box collider around the mesh for AR Raycast interactions.... It sort of works?
        //todo: I think the collider needs to be a child object?
        var box = createdModel.AddComponent<BoxCollider>();
        var mesh = createdModel.GetComponent<MeshRenderer>();
        box.center = mesh.bounds.center;
        box.size = mesh.bounds.size;

        var scale = createdModel.AddComponent<ARScaleInteractable>();
        scale.selectMode = InteractableSelectMode.Multiple;
        var rot = createdModel.AddComponent<ARRotationInteractable>();
        rot.selectMode = InteractableSelectMode.Multiple;
        var trans = createdModel.AddComponent<ARTranslationInteractable>();
        trans.selectMode = InteractableSelectMode.Multiple;
        
        //The AR system SHOULD attempt to configure itself when it doesnt have defaults...
        //If not, we would spawn in a prefab that is configured correctly, sans mesh/collider and set the created object (plus its collider) as a child of it.
    }
}
