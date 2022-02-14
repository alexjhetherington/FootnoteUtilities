using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LayerGroup : ScriptableObject
{
    [SerializeField]
    private string[] layers;

    public int GetMask()
    {
        foreach (string layer in layers)
        {
            if (LayerMask.NameToLayer(layer) == -1)
                Debug.LogError("Layer " + layer + " referenced in LayerGroup does not exist!");
        }

        return LayerMask.GetMask(layers);
    }
}
