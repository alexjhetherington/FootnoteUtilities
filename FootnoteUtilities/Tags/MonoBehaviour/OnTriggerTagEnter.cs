using UnityEngine;
using UnityEngine.Events;

public class OnTriggerTagEnter : MonoBehaviour
{
    public Tag checkTag;
    public bool compound = false;
    public UnityEvent unityEvent;

    void Awake()
    {
        if (compound && GetComponent<CompoundTrigger>() == null)
        {
            Debug.LogError(
                gameObject.name
                    + " with compound trigger flag does not have a compound trigger component",
                this
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (compound)
            return;

        if (other.HasTag(checkTag))
        {
            unityEvent.Invoke();
        }
    }

    void OnCompoundTriggerEnter(Collider other)
    {
        if (!compound)
            return;

        if (other.HasTag(checkTag))
        {
            unityEvent.Invoke();
        }
    }
}
