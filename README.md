# AR-Mobile-Model-Importer-and-Viewer
Demo to figure out how to load models into unity, at runtime, and view in AR

---

Loading 3D models from the file system turns out to be non-trivial.



1. Native File Picker. I'm solving this with: [yasirkula/UnityNativeFilePicker](https://github.com/yasirkula/UnityNativeFilePicker)
2. Parseing the raw data into an asset. For OBJ I'm using this: [codemaker2015/unity3d-runtime-model-importer](https://github.com/codemaker2015/unity3d-runtime-model-importer).

I want to figure out FBX next, but the best look for that appears to be [UniFBX 2](https://assetstore.unity.com/packages/tools/modeling/unifbx-2-61108), and I don't feel like buying it. I think it might also be possible to use [FBXSharp](https://github.com/izrik/FbxSharp).


For AR Interaction. Another next step is to bring the asset in as a prefab or hidden object, then use it with the [AR Placement interactable](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/ar-placement-interactable.html).
Also, I don't have the AR Interactors working right now.


