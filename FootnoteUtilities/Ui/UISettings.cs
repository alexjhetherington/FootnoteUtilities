using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AnchorUtil;
using static UnityEngine.UI.Slider;

[CreateAssetMenu()]
public class UISettings : ScriptableObject
{
    private static Color buttonAdjust = new Color(0.1f, 0.1f, 0.1f, 0f);

    [Header("Title Text")]
    [SerializeField]
    private TextAlignmentOptions titleAlignment;
    [SerializeField]
    private TMP_FontAsset titleFont;
    [SerializeField]
    private Color titleForeground = Color.white;
    [SerializeField]
    private float titleSize = 32;
    [SerializeField]
    private float titleBottomMargin = 8;

    [Header("Other Text")]
    [SerializeField]
    private TextAlignmentOptions textAligntment;
    [SerializeField]
    private TMP_FontAsset textFont;
    [SerializeField]
    private Color textForeground = Color.white;
    [SerializeField]
    private float textSize = 24;

    [Header("Panel and Spacing")]
    [SerializeField]
    private Color panelBackground = Color.black;
    [SerializeField]
    private Sprite panelSprite;
    [SerializeField]
    private int padding = 20;
    [SerializeField]
    private int verticalSpacing = 4;

    [Header("Buttons")]
    [SerializeField]
    private TextAnchor buttonAlignment;
    [SerializeField]
    private Color buttonBackground = Color.white - buttonAdjust;
    [SerializeField]
    private Color buttonForeground = Color.black;
    [SerializeField]
    private Sprite buttonSprite;
    [SerializeField]
    private int buttonPadding = 4;
    [SerializeField]
    private Vector2 overrideButtonSize = default;

    [Header("Toggles")]
    [SerializeField]
    private Vector2 toggleSize = new Vector2(20, 20);
    [SerializeField]
    private Sprite toggleSprite;
    [SerializeField]
    private Color toggleBackground = Color.grey;
    [SerializeField]
    private Sprite checkmarkSprite;
    [SerializeField]
    private Color checkMarkColor = Color.green;

    [Header("Slider")]
    [SerializeField]
    private Color sliderBackground = Color.grey;
    [SerializeField]
    private Sprite sliderSprite;
    [SerializeField]
    private Color sliderFill = Color.green;
    [SerializeField]
    private Sprite sliderFillSprite;
    [SerializeField]
    private Color sliderHandle = Color.white;
    [SerializeField]
    private Sprite sliderHandleSprite;
    [SerializeField]
    private Vector2 sliderSize = new Vector2(100, 15);
    [SerializeField]
    private Vector2 sliderHandleSize = new Vector2(15, 15);

    public RectTransform MakeUi(AnchorPosParams anchorPos)
    {
        if (EventSystem.current == null)
        {
            GameObject eventGo = new GameObject(
                "FootnotesUi Event System",
                typeof(EventSystem),
                typeof(StandaloneInputModule)
            );
        }

        GameObject rootGo = new GameObject("FootnotesUi Root", typeof(RectTransform));
        rootGo.layer = LayerMask.NameToLayer("UI");

        GameObject camGo = new GameObject("FootnotesUi Camera");
        Camera cam = camGo.AddComponent<Camera>();
        Canvas canvas = rootGo.AddComponent<Canvas>();
        GraphicRaycaster raycaster = rootGo.AddComponent<GraphicRaycaster>();

        cam.clearFlags = CameraClearFlags.Nothing;
        cam.depth = 50;
        cam.cullingMask = 1 << LayerMask.NameToLayer("UI");

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 10;

        var basePanel = Panel(anchorPos).SetAnchorPos(anchorPos);
        rootGo.GetComponent<RectTransform>().AddChildren(basePanel);

        return basePanel;
    }

    public RectTransform Title(string text)
    {
        var title = UiGo("Title");
        var textMesh = title.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = titleForeground;
        textMesh.fontSize = titleSize;
        textMesh.margin = new Vector4(0, 0, 0, titleBottomMargin);
        textMesh.alignment = titleAlignment;

        if (titleFont != null)
        {
            textMesh.font = titleFont;
        }

        //title.AddPreferredSizing();
        return title;
    }

    public RectTransform Text(string text, Color? color = null, TextAlignmentOptions? overrideAlignment = null)
    {
        var normal = UiGo("Text");
        var textMesh = normal.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color.GetValueOrDefault(textForeground);
        textMesh.fontSize = textSize;

        textMesh.alignment = overrideAlignment.GetValueOrDefault(textAligntment);

        if (textFont != null)
        {
            textMesh.font = textFont;
        }

        //normal.AddPreferredSizing();
        return normal;
    }

    public RectTransform Button(string text, UnityAction action)
    {
        return Button(text, null, null, action);
    }

    public RectTransform Button(string text, Color? overrideColor, Sprite overrideSprite, UnityAction action)
    {
        var container = UiGo("Button Layout Container");

        var button = UiGo("Button");
        var image = button.AddComponent<Image>();

        if(overrideSprite != null)
        {
            image.sprite = overrideSprite;
        }
        else if (buttonSprite != null)
        {
            image.sprite = buttonSprite;
        }

        var b = button.AddComponent<Button>();
        b.targetGraphic = image;
        var colours = b.colors;
        if (overrideColor.HasValue)
        {
            colours.normalColor = overrideColor.Value;
            colours.highlightedColor = overrideColor.Value + buttonAdjust;
            colours.pressedColor = overrideColor.Value;
        }
        else
        {
            colours.normalColor = buttonBackground;
            colours.highlightedColor = buttonBackground + buttonAdjust;
            colours.pressedColor = buttonBackground;
        }
        b.colors = colours;

        var layout = button.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.padding.left = buttonPadding;
        layout.padding.top = buttonPadding;
        layout.padding.right = buttonPadding;
        layout.padding.bottom = buttonPadding;

        if(overrideButtonSize != default)
        {
            var layoutElement = button.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = overrideButtonSize.y;
            layoutElement.preferredWidth = overrideButtonSize.x;
        }

        b.onClick.AddListener(action);

        button.AddChildren(Text(text, buttonForeground));
        button.AddPreferredSizing();

        var textComp = button.GetComponentInChildren<TextMeshProUGUI>();
        textComp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textComp.verticalAlignment = VerticalAlignmentOptions.Middle;
        textComp.enableWordWrapping = false;

        container.AddChildren(button);
        var containerLayout = container.AddComponent<HorizontalLayoutGroup>();
        containerLayout.childControlHeight = false;
        containerLayout.childControlWidth = false;
        containerLayout.childAlignment = buttonAlignment;

        return container;
    }

    public RectTransform Toggle(
        string text,
        UnityAction<bool> action,
        out UnityAction<bool> changeValue
    )
    {
        var toggle = UiGo("Toggle");
        var layout = toggle.AddComponent<HorizontalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        toggle.SetAnchorPos(AnchorUtil.TopHorizontalStretch());

        var txt = Text(text, null, TextAlignmentOptions.MidlineLeft);
        toggle.AddChildren(txt);

        var backgroundGo = UiGo("Background");
        var backgroundImage = backgroundGo.AddComponent<Image>();
        backgroundGo.SetAnchorPos(AnchorUtil.CentreRight(0));
        backgroundImage.color = toggleBackground;
        if (toggleSprite != null)
        {
            backgroundImage.sprite = toggleSprite;
        }
        backgroundGo.sizeDelta = toggleSize;

        var checkMarkGo = UiGo("CheckMark");
        var checkmarkBackground = checkMarkGo.AddComponent<Image>();
        checkmarkBackground.color = checkMarkColor;
        if (checkmarkSprite != null)
        {
            checkmarkBackground.sprite = checkmarkSprite;
        }
        checkMarkGo.sizeDelta = toggleSize;

        txt.AddChildren(backgroundGo);
        backgroundGo.AddChildren(checkMarkGo);

        var t = toggle.AddComponent<Toggle>();
        t.image = backgroundImage;
        t.graphic = checkmarkBackground;

        t.onValueChanged.AddListener(action);

        changeValue = b => t.SetIsOnWithoutNotify(b);

        return toggle;
    }

    /*public RectTransform DropDown(
        string text,
        string[] options,
        UnityAction<int> action,
        out UnityAction<int> changeValue
    )
    {
        throw new NotImplementedException();
        var dropDown = UiGo("Toggle");
        return dropDown;
    }*/

    public RectTransform Slider(
        string text,
        float left,
        float right,
        bool wholeNumbers,
        UnityAction<float> action,
        out UnityAction<float> changeValue
    )
    {
        var topLevel = Text(text, null, TextAlignmentOptions.MidlineLeft);
        topLevel.gameObject.name = "Slider";
        var bk = UiGo("Background");
        var fill = UiGo("Fill");
        var handle = UiGo("Handle");

        topLevel.AddChildren(bk);
        bk.AddChildren(fill, handle);

        bk.SetAnchorPos(AnchorUtil.CentreRight(0));
        bk.sizeDelta = sliderSize;

        fill.SetAnchorPos(AnchorUtil.FullScreenStretch());
        handle.sizeDelta = new Vector2(sliderHandleSize.x, sliderHandleSize.y - sliderSize.y);

        var handleImage = handle.AddComponent<Image>();
        var bkImage = bk.AddComponent<Image>();
        var fillImage = fill.AddComponent<Image>();

        handleImage.color = sliderHandle;
        bkImage.color = sliderBackground;
        fillImage.color = sliderFill;

        if (sliderHandleSprite != null)
        {
            handleImage.sprite = sliderHandleSprite;
        }
        if (sliderFillSprite != null)
        {
            fillImage.sprite = sliderFillSprite;
        }
        if (sliderSprite != null)
        {
            bkImage.sprite = sliderSprite;
        }

        var slider = bk.AddComponent<Slider>();

        slider.minValue = left;
        slider.maxValue = right;
        slider.wholeNumbers = wholeNumbers;
        slider.direction = Direction.LeftToRight;

        slider.targetGraphic = handleImage;
        slider.handleRect = handle;
        slider.fillRect = fill;

        slider.onValueChanged.AddListener(action);
        changeValue = f => slider.SetValueWithoutNotify(f);
        return topLevel;
    }

    private RectTransform Panel(AnchorUtil.AnchorPosParams anchorPos)
    {
        var panel = UiGo("Panel").SetAnchorPos(anchorPos);
        panel.AddPreferredSizing();
        var layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.padding.left = padding;
        layout.padding.top = padding;
        layout.padding.right = padding;
        layout.padding.bottom = padding;
        layout.spacing = verticalSpacing;

        var image = panel.AddComponent<Image>();

        if (panelSprite != null)
        {
            image.sprite = panelSprite;
        }

        image.color = panelBackground;
        return panel;
    }

    public RectTransform Nest()
    {
        var panel = UiGo("Nest").SetAnchorPos(AnchorUtil.TopLeft(0, 0));
        panel.AddPreferredSizing();
        var layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.padding.left = padding;
        layout.padding.top = padding;
        layout.padding.right = padding;
        layout.padding.bottom = padding;
        layout.spacing = verticalSpacing;
        return panel;
    }

    public RectTransform UiGo(string name, params RectTransform[] children)
    {
        GameObject uiGo = new GameObject(name, typeof(RectTransform));
        RectTransform rt = uiGo.GetComponent<RectTransform>();
        rt.AddChildren(children);
        return rt;
    }

    /* Functions as a quick start guide, and allows you to create a UI in the editor to see what it looks like */
    /* This framework was initially designed for UIs that are created at runtime and destroyed on scene change*/
    /* If you want similar behaviour for your real UI (not just the below test UI), make a similar method in your own class */
    /* Note: Actions are not persisted into play mode if the UI was created in editor mode! */
    [Button]
    private void CreateTestUi()
    {
        //In normal usage, handle the lifecycle yourself (or just let them be destroyed/created on scene load)
        DestroyImmediate(GameObject.Find("FootnotesUi Camera"));
        DestroyImmediate(GameObject.Find("FootnotesUi Root"));
        DestroyImmediate(GameObject.Find("FootnotesUi Event System"));

        //Put a method like this in your own class
        var ui = this.MakeUi(AnchorUtil.BottomLeft(40, 40))
            .AddChildren(
                this.Title("Title"),
                this.Text("This is main line 1"),
                this.Nest()
                    .AddChildren(
                        this.Text("This is nested line 1"),
                        this.Text("This is nested line 2"),
                        this.Nest()
                            .AddChildren(
                                this.Text("This is dobule nested line 1"),
                                this.Text("This is double nested line 2"),
                                this.Button("Button", () => Debug.Log("Button Pressed")),
                                this.Toggle("Toggle", b => Debug.Log(b), out var act2),
                                this.Slider("Slider", 0, 1, false, f => Debug.Log(f), out var act3)
                    )),
                this.Text("This is main line 2"),
                this.Button("Button", () => Debug.Log("Button Pressed")),
                this.Toggle("Toggle", b => Debug.Log(b), out var act4),
                this.Slider("Slider", 0, 1, false, f => Debug.Log(f), out var act5)
            );

        act2.Invoke(true); //Set toggle state
        act3.Invoke(0.5f); //Set slider state

        //This is not required in play mode :)
        LayoutRebuilder.ForceRebuildLayoutImmediate(ui);
    }
}
