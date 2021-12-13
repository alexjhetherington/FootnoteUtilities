﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Transitions
{
    private static Dictionary<int, Transition> transitions = new Dictionary<int, Transition>();

    public static void Start(string transitionScene, Action onScreenObscured)
    {
        Start(SceneManagerUtilities.GetBuildIndexByName(transitionScene), onScreenObscured, -1);
    }

    public static void Start(string transitionScene, string nextScene)
    {
        Start(SceneManagerUtilities.GetBuildIndexByName(transitionScene), null, SceneManagerUtilities.GetBuildIndexByName(nextScene));
    }

    public static void Start(string transitionScene, int nextScene)
    {
        Start(SceneManagerUtilities.GetBuildIndexByName(transitionScene), null, nextScene);
    }

    public static void Start(string transitionScene, Action onScreenObscured, string nextScene)
    {
        Start(SceneManagerUtilities.GetBuildIndexByName(transitionScene), onScreenObscured, SceneManagerUtilities.GetBuildIndexByName(nextScene));
    }
    
    public static void Start(int transitionScene, Action onScreenObscured)
    {
        Start(transitionScene, onScreenObscured, -1);
    }

    public static void Start(int transitionScene, int nextScene)
    {
        Start(transitionScene, null, nextScene);
    }

    public static void Start(int transitionScene, Action onScreenObscured, int nextScene)
    {
        Debug.Log(SceneUtility.GetScenePathByBuildIndex(transitionScene));
        if (transitions.ContainsKey(transitionScene))
        {
            DoTransition(transitionScene, onScreenObscured, nextScene);
        }
        else
        {
            ScenePack.UnpackScene<Transition>(transitionScene, GetUnpackedHandler(transitionScene, onScreenObscured, nextScene));
            
        }
    }

    private static void DoTransition(int transitionScene, Action onScreenObscured, int nextScene)
    {
        Transition transition = transitions[transitionScene];
        transition.Obscure(() => {
            onScreenObscured?.Invoke();

            if(nextScene >= 0)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
                Coroutiner.Instance.StartCoroutine(WaitForSceneLoad(transition, asyncLoad));
            }
            else
            {
                transition.Unobscure();
            }
        });
    }

    private static IEnumerator WaitForSceneLoad(Transition transition, AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
            yield return null;

        transition.Unobscure();
    }

    private static Action<Transition> GetUnpackedHandler(int transitionScene, Action onScreenObscured, int nextScene)
    {
        return transition =>
        {
            transitions.Add(transitionScene, transition);
            DoTransition(transitionScene, onScreenObscured, nextScene);
        };
    }
}
