using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStringOnEnable : MonoBehaviour
{
    public StringVariable stringVariable;
    public string value;

    void OnEnable()
    {
        stringVariable.Value = value;
    }
}
