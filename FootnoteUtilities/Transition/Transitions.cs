using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Transitions
{
    private static Dictionary<int, Transition> transitions = new Dictionary<int, Transition>();

    private static bool inTransition = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Init()
    {
        transitions = new Dictionary<int, Transition>();
        inTransition = false;
    }

    public static void Start(string transitionScene, Action onScreenObscured)
    {
        Start(
            SceneManagerUtilities.GetBuildIndexByName(transitionScene),
            onScreenObscured,
            -1,
            null
        );
    }

    public static void Start(string transitionScene, string nextScene)
    {
        Start(
            SceneManagerUtilities.GetBuildIndexByName(transitionScene),
            null,
            SceneManagerUtilities.GetBuildIndexByName(nextScene),
            null
        );
    }

    public static void Start(string transitionScene, int nextScene)
    {
        Start(SceneManagerUtilities.GetBuildIndexByName(transitionScene), null, nextScene, null);
    }

    public static void Start(string transitionScene, Action onScreenObscured, string nextScene)
    {
        Start(
            SceneManagerUtilities.GetBuildIndexByName(transitionScene),
            onScreenObscured,
            SceneManagerUtilities.GetBuildIndexByName(nextScene),
            null
        );
    }

    public static void Start(
        string transitionScene,
        string nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        Start(
            SceneManagerUtilities.GetBuildIndexByName(transitionScene),
            null,
            SceneManagerUtilities.GetBuildIndexByName(nextScene),
            onSceneLoadedAndObscured
        );
    }

    public static void Start(
        string transitionScene,
        Action onScreenObscured,
        string nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        Start(
            SceneManagerUtilities.GetBuildIndexByName(transitionScene),
            onScreenObscured,
            SceneManagerUtilities.GetBuildIndexByName(nextScene),
            onSceneLoadedAndObscured
        );
    }

    public static void Start(int transitionScene, Action onScreenObscured)
    {
        Start(transitionScene, onScreenObscured, -1, null);
    }

    public static void Start(int transitionScene, int nextScene)
    {
        Start(transitionScene, null, nextScene, null);
    }

    public static void Start(
        int transitionScene,
        Action onScreenObscured,
        int nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        Debug.Log(SceneUtility.GetScenePathByBuildIndex(transitionScene));
        if (transitions.ContainsKey(transitionScene))
        {
            DoTransition(transitionScene, onScreenObscured, nextScene, onSceneLoadedAndObscured);
        }
        else
        {
            Coroutiner.Instance.StartCoroutine(
                LoadThenDoTransition(
                    transitionScene,
                    onScreenObscured,
                    nextScene,
                    onSceneLoadedAndObscured
                )
            );
        }
    }

    private static IEnumerator LoadThenDoTransition(
        int transitionScene,
        Action onScreenObscured,
        int nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        var transition = StaticSceneHelper.GetComponentFromSceneAsStatic<Transition>(
            StaticSceneHelper.LoadType.Async,
            transitionScene
        );
        yield return transition;
        transitions[transitionScene] = transition.value;
        DoTransition(transitionScene, onScreenObscured, nextScene, onSceneLoadedAndObscured);
    }

    private static void DoTransition(
        int transitionScene,
        Action onScreenObscured,
        int nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        if (inTransition)
        {
            Debug.LogWarning("Tried to start a transition while one was in progress");
            return;
        }

        inTransition = true;
        Transition transition = transitions[transitionScene];
        transition.Obscure(
            () =>
            {
                onScreenObscured?.Invoke();

                if (nextScene >= 0)
                {
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
                    Coroutiner.Instance.StartCoroutine(
                        WaitForSceneLoad(onSceneLoadedAndObscured, transition, asyncLoad)
                    );
                }
                else
                {
                    transition.Unobscure(() => inTransition = false);
                }
            }
        );
    }

    private static IEnumerator WaitForSceneLoad(
        Action onSceneLoadedAndObscured,
        Transition transition,
        AsyncOperation asyncLoad
    )
    {
        while (!asyncLoad.isDone)
            yield return null;

        onSceneLoadedAndObscured?.Invoke();
        transition.Unobscure(() => inTransition = false);
    }

    private static Action<Transition> GetUnpackedHandler(
        int transitionScene,
        Action onScreenObscured,
        int nextScene,
        Action onSceneLoadedAndObscured
    )
    {
        return transition =>
        {
            transitions.Add(transitionScene, transition);
            DoTransition(transitionScene, onScreenObscured, nextScene, onSceneLoadedAndObscured);
        };
    }
}
