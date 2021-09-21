using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParentNode
{
    void HandleChildComplete();
    void HandleChildFailed();
    void HandleChildInterrupt(Node child); //Force reevaluation
}
