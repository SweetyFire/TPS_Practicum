using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialToObject
{
    public List<MeshRenderer> objects = new();
    public Material material;
}

[ExecuteInEditMode]
public class MaterialApplicator : MonoBehaviour
{
    [SerializeField] private List<MaterialToObject> _materials = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_materials.Count <= 0)
        {
            _materials.Add(new MaterialToObject());
        }

        foreach (MaterialToObject mat in _materials)
        {
            if (mat.material == null) continue;

            foreach (MeshRenderer obj in mat.objects)
            {
                obj.sharedMaterial = mat.material;
            }
        }
    }
#endif
}
