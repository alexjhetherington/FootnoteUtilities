using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface Transition
{
    void Obscure(Action onScreenObscured);
    void Unobscure(Action onScreenUnobscured);
}
