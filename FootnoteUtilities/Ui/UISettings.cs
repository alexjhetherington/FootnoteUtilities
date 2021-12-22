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
    [Header("Title Text")]
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
    private Color buttonBackground = Color.white - buttonAdjust;
    [SerializeField]
    private Color buttonForeground = Color.black;
    [SerializeField]
    private Sprite buttonSprite;
    [SerializeField]
    private int buttonPadding = 4;

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
                "Event System",
                typeof(EventSystem),
                typeof(StandaloneInputModule)
            );
        }

        GameObject rootGo = new GameObject("Ui Root", typeof(RectTransform));
        rootGo.layer = LayerMask.NameToLayer("UI");

        GameObject camGo = new GameObject("Ui Camera");
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

        if (titleFont != null)
        {
            textMesh.font = titleFont;
        }

        //title.AddPreferredSizing();
        return title;
    }

    public RectTransform Text(string text, Color? color = null)
    {
        var normal = UiGo("Text");
        var textMesh = normal.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color.GetValueOrDefault(textForeground);
        textMesh.fontSize = textSize;

        if (textFont != null)
        {
            textMesh.font = textFont;
        }

        //normal.AddPreferredSizing();
        return normal;
    }

    private static Color buttonAdjust = new Color(0.1f, 0.1f, 0.1f, 1f);
    public RectTransform Button(string text, UnityAction action)
    {
        var container = UiGo("Button Layout Container");

        var button = UiGo("Button");
        var image = button.AddComponent<Image>();

        if (buttonSprite != null)
        {
            image.sprite = buttonSprite;
        }

        var b = button.AddComponent<Button>();
        b.targetGraphic = image;
        var colours = b.colors;
        colours.normalColor = buttonBackground;
        colours.highlightedColor = buttonBackground + buttonAdjust;
        colours.pressedColor = buttonBackground;
        b.colors = colours;

        var layout = button.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.padding.left = buttonPadding;
        layout.padding.top = buttonPadding;
        layout.padding.right = buttonPadding;
        layout.padding.bottom = buttonPadding;

        b.onClick.AddListener(action);

        button.AddChildren(Text(text, buttonForeground));
        button.AddPreferredSizing();

        container.AddChildren(button);
        var containerLayout = container.AddComponent<HorizontalLayoutGroup>();
        containerLayout.childControlHeight = false;
        containerLayout.childControlWidth = false;
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

        var txt = Text(text);
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
        var topLevel = Text(text);
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
}
