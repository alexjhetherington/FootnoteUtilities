using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Recommend to be used with OnTriggerTagEnter and OnTriggerTagExit
public class SimpleCullingGroup : MonoBehaviour
{
    public GameObject[] contents;
    private Coroutine coroutine;
    private int defaultFrameDelay = 2;

    int viewers = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cull(true, 1);
    }

    public void AddViewer()
    {
        AddViewer(defaultFrameDelay);
    }

    public void RemoveViewer()
    {
        RemoveViewer(defaultFrameDelay);
    }

    public void AddViewer(int frameDelay)
    {
        if (viewers++ == 0)
            Cull(false, frameDelay);
    }

    public void RemoveViewer(int frameDelay)
    {
        if (--viewers == 0)
            Cull(true, frameDelay);
    }

    private void Cull(bool value, int frameDelay)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(CullOverFrames(value, frameDelay));
    }

    IEnumerator CullOverFrames(bool value, int frameDelay)
    {
        foreach (GameObject go in contents)
        {
            for (int i = 0; i < frameDelay; i++)
            {
                yield return null;
            }

            go.SetActive(!value);
        }
    }
}
