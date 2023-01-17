using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Menus : MonoBehaviour
{
    [SerializeField]
    private AudioMixer fxMixer;
    [SerializeField]
    private string fxVolParam;

    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private string musicVolParam;

    [SerializeField]
    private UISettings uiSettings;

    private List<RectTransform> currentUis = new List<RectTransform>();

    public void Start()
    {
        if (fxMixer != null)
            LoadVolume(fxMixer, fxVolParam, f => { }); //Doesn't work in Awake

        if (musicMixer != null)
            LoadVolume(musicMixer, musicVolParam, f => { }); //Doesn't work in Awake
    }

    public void ClearMenus()
    {
        foreach (RectTransform ui in currentUis)
        {
            UISettings.DestroyUi(ui);
        }

        currentUis.Clear();
    }

    public void MainMenu()
    {
        //TODO - Change the main menu!
        var ui = uiSettings
            .MakeUi(AnchorUtil.BottomLeft(40, 40))
            .AddChildren(
                uiSettings.Title("Game Name"),
                uiSettings.Button("Play", () => Transitions.Start("SimpleFade", "Game")),
                uiSettings.Button("Options", () => OptionsMenu(() => { })),
                uiSettings.Button("Quit", () => Application.Quit())
            );

        currentUis.Add(ui);
    }

    public void PauseMenu(Pause pause)
    {
        var ui = uiSettings
            .MakeUi(AnchorUtil.BottomLeft(40, 40))
            .AddChildren(
                uiSettings.Title("Paused"),
                uiSettings.Button("Resume", () => pause.Unpause()),
                uiSettings.Button("Options", () => OptionsMenu(() => { })),
                uiSettings.Button(
                    "Return to Menu",
                    () =>
                    {
                        Time.timeScale = 1;
                        Transitions.Start("SimpleFade", "MainMenu");
                    }
                )
            );

        currentUis.Add(ui);
    }

    public void OptionsMenu(Action onClose)
    {
        var ui = uiSettings.MakeUi(AnchorUtil.Centre(0, 0));

        var options = new List<RectTransform>();

        if (fxMixer != null)
        {
            options.Add(
                uiSettings.Slider(
                    "FX Volume",
                    0,
                    10,
                    false,
                    null,
                    f => SetVolume(fxMixer, fxVolParam, f),
                    out var changeFxSlider
                )
            );

            LoadVolume(fxMixer, fxVolParam, f => changeFxSlider.value = f);
        }

        if (musicMixer != null)
        {
            options.Add(
                uiSettings.Slider(
                    "Music Volume",
                    0,
                    10,
                    false,
                    null,
                    f => SetVolume(musicMixer, musicVolParam, f),
                    out var changeMusicSlider
                )
            );

            LoadVolume(musicMixer, musicVolParam, f => changeMusicSlider.value = f);
        }

        ui.AddChildren(
            uiSettings.Title("Options"),
            uiSettings.Nest().AddChildren(options.ToArray()),
            uiSettings.Button(
                "Back",
                () =>
                {
                    currentUis.Remove(ui);
                    UISettings.DestroyUi(ui);
                    onClose.Invoke();
                }
            )
        );

        currentUis.Add(ui);
    }

    private void SetVolume(AudioMixer mixer, string param, float value)
    {
        PlayerPrefs.SetFloat(param, value);

        mixer.SetFloat(param, FootnoteUnits.linearToDecibels(value));
    }

    private void LoadVolume(AudioMixer mixer, string param, Action<float> setSlider)
    {
        var volume = PlayerPrefs.GetFloat(param, 10);

        SetVolume(mixer, param, volume);
        setSlider.Invoke(volume);
    }
}
