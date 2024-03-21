using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabBounds : MonoBehaviour
{
    private MeshRenderer[] meshes;
    
    public Vector3 GetSize()
    {
        meshes = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        if (meshes.Length == 0)
        {
            return Vector3.zero;
        }
        
        Bounds bounds = new Bounds();

        for (int i = 0; i < meshes.Length; i++)
        {
            bounds.Encapsulate(meshes[i].bounds);
        }
        
        return bounds.size;
    }
    
    
}
