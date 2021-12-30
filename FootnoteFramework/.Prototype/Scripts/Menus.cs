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

    public void Start()
    {
        if (fxMixer != null)
            LoadVolume(fxMixer, fxVolParam, f => { }); //Doesn't work in Awake

        if (musicMixer != null)
            LoadVolume(musicMixer, musicVolParam, f => { }); //Doesn't work in Awake
    }

    public void MainMenu()
    {
        //TODO - Change the main menu!
        var ui = uiSettings
            .MakeUi(AnchorUtil.BottomLeft(40, 40))
            .AddChildren(
                uiSettings.Title("Game Name"),
                uiSettings.Button(
                    "Play",
                    () => Debug.LogWarning("This button doesn't do anything yet!")
                ),
                uiSettings.Button("Options", () => OptionsMenu(() => { })),
                uiSettings.Button("Quit", () => Application.Quit())
            );
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
                    f => SetVolume(fxMixer, fxVolParam, f),
                    out var changeFxSlider
                )
            );

            LoadVolume(fxMixer, fxVolParam, f => changeFxSlider.Invoke(f));
        }

        if (musicMixer != null)
        {
            options.Add(
                uiSettings.Slider(
                    "Music Volume",
                    0,
                    10,
                    false,
                    f => SetVolume(musicMixer, musicVolParam, f),
                    out var changeMusicSlider
                )
            );

            LoadVolume(musicMixer, musicVolParam, f => changeMusicSlider.Invoke(f));
        }

        ui.AddChildren(
            uiSettings.Title("Options"),
            uiSettings.Nest().AddChildren(options.ToArray()),
            uiSettings.Button(
                "Back",
                () =>
                {
                    UISettings.DestroyUi(ui);
                    onClose.Invoke();
                }
            )
        );
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
