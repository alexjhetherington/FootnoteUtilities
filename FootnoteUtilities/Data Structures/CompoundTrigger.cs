using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundTrigger : MonoBehaviour
{
    Dictionary<Collider, int> m_Colliders = new Dictionary<Collider, int>();

    void OnTriggerEnter(Collider aOther)
    {
        if (m_Colliders.ContainsKey(aOther))
        {
            m_Colliders[aOther]++;
        }
        else
        {
            SendMessage("OnCompoundTriggerEnter", aOther, SendMessageOptions.DontRequireReceiver);
            m_Colliders.Add(aOther, 1);
        }
    }
    void OnTriggerExit(Collider aOther)
    {
        if (m_Colliders.ContainsKey(aOther))
        {
            m_Colliders[aOther]--;
            if (m_Colliders[aOther] <= 0)
            {
                m_Colliders.Remove(aOther);
                SendMessage(
                    "OnCompoundTriggerExit",
                    aOther,
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }
        else
            Debug.LogError("This should never happen");
    }
}
