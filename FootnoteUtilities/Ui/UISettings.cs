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

/* Helpful things still to be added:
    * Manually adding line breaks */
[CreateAssetMenu()]
public class UISettings : ScriptableObject
{
    [Header("Title Text")]
    [SerializeField]
    private TextAlignmentOptions titleAlignment = TextAlignmentOptions.MidlineLeft;
    [SerializeField]
    private TMP_FontAsset titleFont;
    [SerializeField]
    private Color titleForeground = Color.white;
    [SerializeField]
    private float titleSize = 32;
    [SerializeField]
    private float titleTopmargin = 8;
    [SerializeField]
    private float titleBottomMargin = 8;

    [Header("Other Text")]
    [SerializeField]
    private TextAlignmentOptions textAligntment = TextAlignmentOptions.MidlineLeft;
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
    private Material panelMaterial;
    [SerializeField]
    private int padding = 20;
    [SerializeField]
    private int verticalSpacing = 4;
    [SerializeField]
    private int minTextWidgetDistance = 20;

    [Header("Buttons")]
    [SerializeField]
    private TextAnchor buttonAlignment;
    [SerializeField]
    private Color buttonBackground = Color.white;
    [SerializeField]
    private Color buttonForeground = Color.black;
    [SerializeField]
    private Color buttonHighlight = Color.white;
    [SerializeField]
    private Sprite buttonSprite;
    [SerializeField]
    private Vector4 buttonPadding = new Vector4(4, 4, 4, 4);
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
    private float sliderDisplayedValueWidth = 50f;
    [SerializeField]
    private Vector2 sliderSize = new Vector2(100, 15);
    [SerializeField]
    private Vector2 sliderHandleSize = new Vector2(15, 15);

    [Header("Scrollbar")]
    [SerializeField]
    private Color scrollbarBackground = Color.grey;
    [SerializeField]
    private Sprite scrollbarSprite;
    [SerializeField]
    private Color scrollbarHandle = Color.white;
    [SerializeField]
    private Sprite scrollbarHandleSprite;

    [SerializeField]
    private int scrollbarWidth = 20;

    [SerializeField]
    private int scrollbarInnerPadding = 0;

    [Header("Camera")]
    [SerializeField]
    private Rect viewport;
    [SerializeField]
    private Color cameraBackground;

    [SerializeField]
    private Vector2 screenSizeScaleReference = default;
    [SerializeField]
    private float widthToHeightScaling = 0;

    //Options, height stretch/maxHeight, width stretched, autoWidth
    //Height Stretch, Width Stretch -- Full screen and nested items will be stretched to fit
    //MaxHeight, Width Stretch -- Height is limited and nested items will be stretched to fit; rare case because small box with fixed width is rare
    //Height Stretch, AutoWidth -- Full screen and nested and nested items choose their own size
    //MaxHeight, AutoWidth -- Normal case for small boxes

    public RectTransform MakeFullScreenScrollUi(ContentSizeFitter.FitMode contentWidthFitMode)
    {
        return MakeScrollUi(AnchorUtil.FullScreenStretch(), contentWidthFitMode);
    }

    public RectTransform MakeAnchoredScrollUiWithMaxHeight(
        AnchorPosParams anchorPosParams,
        float maxHeight
    )
    {
        return MakeScrollUi(anchorPosParams, ContentSizeFitter.FitMode.PreferredSize, maxHeight);
    }

    private RectTransform MakeScrollUi(
        AnchorPosParams anchorPos,
        ContentSizeFitter.FitMode contentWidthFitMode = ContentSizeFitter.FitMode.PreferredSize,
        float maxHeight = 0
    )
    {
        //If not stretched, height must be defined
        if (anchorPos.anchorMin.y == anchorPos.anchorMax.y && maxHeight == 0)
        {
            Debug.LogError("Unable to create non-stretched Scroll UI without max height");
            throw new Exception();
        }

        //If stretched, height will be ignored
        if (anchorPos.anchorMin.y != anchorPos.anchorMax.y && maxHeight != 0)
        {
            Debug.LogWarning(
                "Creating stretched Scroll UI will ignore maxHeight. Perhaps you want to use MakeFullScreenScrollUi"
            );
        }

        return MakeScrollView(MakeUi(anchorPos), contentWidthFitMode, maxHeight);
    }

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

        cam.clearFlags = CameraClearFlags.Depth;
        cam.depth = 50;
        cam.cullingMask = 1 << LayerMask.NameToLayer("UI");

        if (cameraBackground.a > 0.9)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = cameraBackground;
        }

        if (viewport.size.magnitude > 0.01f)
        {
            cam.rect = viewport;
        }

        if (screenSizeScaleReference.sqrMagnitude > 0.01f)
        {
            CanvasScaler scaler = rootGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = screenSizeScaleReference;
            scaler.matchWidthOrHeight = widthToHeightScaling;
        }

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 10;

        var basePanel = Panel(anchorPos).SetAnchorPos(anchorPos);
        rootGo.GetComponent<RectTransform>().AddChildren(basePanel);

        return basePanel;
    }

    public RectTransform Title(string text, float scale = 1)
    {
        var title = UiGo("Title");
        var textMesh = title.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = titleForeground;
        textMesh.fontSize = titleSize * scale;
        textMesh.margin = new Vector4(0, titleTopmargin * scale, 0, titleBottomMargin * scale);
        textMesh.alignment = titleAlignment;

        if (titleFont != null)
        {
            textMesh.font = titleFont;
        }

        //title.AddPreferredSizing();
        return title;
    }

    public RectTransform Text(
        string text,
        float scale = 1,
        Color? color = null,
        TextAlignmentOptions? overrideAlignment = null
    )
    {
        var normal = UiGo("Text");
        var textMesh = normal.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color.GetValueOrDefault(textForeground);
        textMesh.fontSize = textSize * scale;

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
        return Button(text, null, action);
    }

    public RectTransform Button(
        string text,
        Sprite overrideSprite,
        UnityAction action,
        Color[] overrideColors = default
    )
    {
        var container = UiGo("Button Layout Container");

        var button = UiGo("Button");
        var image = button.AddComponent<Image>();

        if (overrideSprite != null)
        {
            image.sprite = overrideSprite;
            image.type = Image.Type.Sliced;
        }
        else if (buttonSprite != null)
        {
            image.sprite = buttonSprite;
            image.type = Image.Type.Sliced;
        }

        var b = button.AddComponent<Button>();
        b.targetGraphic = image;
        var colours = b.colors;
        if (overrideColors != default)
        {
            colours.normalColor = overrideColors[0];
            colours.highlightedColor = overrideColors[1];
            colours.pressedColor = overrideColors[0];
            colours.selectedColor = overrideColors[0];
            colours.fadeDuration = 0;
        }
        else
        {
            colours.normalColor = buttonBackground;
            colours.highlightedColor = buttonHighlight;
            colours.pressedColor = buttonBackground;
            colours.selectedColor = buttonBackground;
            colours.fadeDuration = 0;
        }
        b.colors = colours;

        var layout = button.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.padding = new RectOffset(
            (int)buttonPadding.x,
            (int)buttonPadding.y,
            (int)buttonPadding.z,
            (int)buttonPadding.w
        );

        if (overrideButtonSize != default)
        {
            var layoutElement = button.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = overrideButtonSize.y;
            layoutElement.preferredWidth = overrideButtonSize.x;
        }

        b.onClick.AddListener(action);

        button.AddChildren(Text(text, 1, buttonForeground));
        button.AddPreferredSizing();

        var textComp = button.GetComponentInChildren<TextMeshProUGUI>();
        textComp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textComp.verticalAlignment = VerticalAlignmentOptions.Capline;
        textComp.enableWordWrapping = false;

        container.AddChildren(button);
        var containerLayout = container.AddComponent<HorizontalLayoutGroup>();
        containerLayout.childControlHeight = true;
        containerLayout.childControlWidth = false;
        containerLayout.childAlignment = buttonAlignment;

        return container;
    }

    public RectTransform Toggle(string text, UnityAction<bool> action, out Toggle toggle)
    {
        var toggleInternal = UiGo("Toggle");
        var layout = toggleInternal.AddComponent<HorizontalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        toggleInternal.SetAnchorPos(AnchorUtil.TopHorizontalStretch());

        var txt = Text(text, 1, null, TextAlignmentOptions.MidlineLeft);
        toggleInternal.AddChildren(txt);

        var textComponent = txt.GetComponent<TextMeshProUGUI>();
        var preferredSize = toggleInternal.AddComponent<LayoutElement>();
        preferredSize.minWidth =
            textComponent.preferredWidth + toggleSize.x + minTextWidgetDistance;

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

        var t = toggleInternal.AddComponent<Toggle>();
        t.image = backgroundImage;
        t.graphic = checkmarkBackground;

        t.onValueChanged.AddListener(action);

        toggle = t;

        return toggleInternal;
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
        Func<float, string> displayValueMapper,
        UnityAction<float> action,
        out Slider slider
    )
    {
        var topLevel = Text(text, 1, null, TextAlignmentOptions.MidlineLeft);
        topLevel.gameObject.name = "Slider";
        var bk = UiGo("Background");
        var fill = UiGo("Fill");
        var handle = UiGo("Handle");

        var textComponent = topLevel.GetComponent<TextMeshProUGUI>();
        var preferredSize = topLevel.AddComponent<LayoutElement>();
        preferredSize.minWidth =
            textComponent.preferredWidth + sliderSize.x + minTextWidgetDistance;

        float sliderOffset = 0;
        TextMeshProUGUI displayedValueTextMesh = default;
        if (displayValueMapper != null)
        {
            var displayedValue = Text("9999", 1, null, TextAlignmentOptions.MidlineRight);
            displayedValueTextMesh = displayedValue.GetComponent<TextMeshProUGUI>();
            topLevel.AddChildren(displayedValue);
            displayedValue.SetAnchorPos(AnchorUtil.CentreRight(0));
            displayedValue.sizeDelta = new Vector2(sliderDisplayedValueWidth, 0);
            sliderOffset = minTextWidgetDistance + sliderDisplayedValueWidth;
        }

        topLevel.AddChildren(bk);
        bk.AddChildren(fill, handle);

        bk.SetAnchorPos(AnchorUtil.CentreRight(sliderOffset));
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
            fillImage.type = Image.Type.Sliced;
        }
        if (sliderSprite != null)
        {
            bkImage.sprite = sliderSprite;
            bkImage.type = Image.Type.Sliced;
        }

        var sliderInternal = bk.AddComponent<Slider>();

        sliderInternal.minValue = left;
        sliderInternal.maxValue = right;
        sliderInternal.wholeNumbers = wholeNumbers;
        sliderInternal.direction = Direction.LeftToRight;

        sliderInternal.targetGraphic = handleImage;
        sliderInternal.handleRect = handle;
        sliderInternal.fillRect = fill;

        sliderInternal.onValueChanged.AddListener(action);
        slider = sliderInternal;

        if (displayedValueTextMesh != null)
        {
            displayedValueTextMesh.text = displayValueMapper(left);
            slider.onValueChanged.AddListener(
                f => displayedValueTextMesh.text = displayValueMapper(f)
            );
        }

        return topLevel;
    }

    //TODO Actually add settings for all these bits
    //TODO Background visuals when panel is smaller than the mask
    private RectTransform MakeScrollView(
        RectTransform content,
        ContentSizeFitter.FitMode contentWidthFitMode,
        float maxHeightIfNotStretch
    )
    {
        //Positions
        var scrollView = UiGo("Scroll Container");
        scrollView.CopyPositionFrom(content);

        // Background Image
        {
            Destroy(content.GetComponent<Image>());

            var imageContainer = UiGo("Image Container");
            //scrollView.AddChildren(imageContainer);
            imageContainer.SetParent(content.parent, false);
            var image = imageContainer.AddComponent<Image>();

            imageContainer.pivot = new Vector2(0, 1);
            imageContainer.anchorMin = new Vector2(0, 1);
            imageContainer.anchorMax = new Vector2(0, 1);

            if (panelSprite != null)
            {
                image.sprite = panelSprite;
                image.type = Image.Type.Sliced;
            }
            if (panelMaterial != null)
            {
                image.material = panelMaterial;
            }

            image.color = panelBackground;

            var panelBackgroundSizer = content.AddComponent<ScrollContentBackgroundSizer>();
            panelBackgroundSizer.imageRect = image.GetComponent<RectTransform>();
            panelBackgroundSizer.containerRect = scrollView;
        }

        scrollView.SetParent(content.parent, false);

        content.SetParent(scrollView);
        content.SetAnchorPos(AnchorUtil.TopHorizontalStretch());

        content.GetComponent<ContentSizeFitter>().horizontalFit = contentWidthFitMode;

        var scrollbarContainer = UiGo("Scroll Bar").SetAnchorPos(AnchorUtil.RightVerticalStretch());
        scrollbarContainer.SetParent(scrollView, false);

        var handle = UiGo("Handle").SetAnchorPos(AnchorUtil.FullScreenStretch());
        handle.offsetMin = new Vector2(scrollbarInnerPadding, 0);
        handle.offsetMax = new Vector2(-scrollbarInnerPadding, 0);

        var slidingArea = UiGo("Sliding Area").SetAnchorPos(AnchorUtil.FullScreenStretch());
        scrollbarContainer.AddChildren(slidingArea);
        slidingArea.AddChildren(handle);

        scrollbarContainer.sizeDelta = new Vector2(scrollbarWidth, scrollView.sizeDelta.y);

        //If not stretched, width must be calculated
        if (scrollView.anchorMin.x == scrollView.anchorMax.x)
        {
            var contentSizeFitter = scrollView.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        //If not stretched, height must be defined
        if (scrollView.anchorMin.y == scrollView.anchorMax.y)
        {
            scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, maxHeightIfNotStretch);
        }

        //var layout = scrollView.AddComponent<HorizontalLayoutGroup>();

        var scrollRect = scrollView.AddComponent<ScrollRect>();
        var mask = scrollView.AddComponent<RectMask2D>();
        var scrollbar = scrollbarContainer.AddComponent<Scrollbar>();
        var scrollBackground = scrollbarContainer.AddComponent<Image>();
        var handleImage = handle.AddComponent<Image>();
        var scrollbarLayout = scrollbarContainer.AddComponent<LayoutElement>();

        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

        scrollBackground.color = scrollbarBackground;
        if (scrollbarSprite != null)
        {
            scrollBackground.sprite = scrollbarSprite;
            scrollBackground.type = Image.Type.Sliced;
        }

        handleImage.color = scrollbarHandle;
        if (scrollbarHandleSprite != null)
        {
            handleImage.sprite = scrollbarHandleSprite;
            handleImage.type = Image.Type.Sliced;
        }

        scrollbarLayout.ignoreLayout = true;

        /*layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.padding.left = -scrollbarWidth / 2;
        layout.padding.right = scrollbarWidth / 2;*/

        scrollRect.content = content;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.viewport = scrollView;
        scrollRect.verticalScrollbar = scrollbar;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        scrollbar.handleRect = handle;
        scrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollRect.scrollSensitivity = 20;

        return content;
    }

    public RectTransform Panel(AnchorUtil.AnchorPosParams anchorPos)
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
            image.type = Image.Type.Sliced;
        }
        if (panelMaterial != null)
        {
            image.material = panelMaterial;
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

    public static void ScrollToTarget(
        ScrollRect scrollRect,
        RectTransform contentPanel,
        RectTransform target
    )
    {
        Canvas.ForceUpdateCanvases();

        if (contentPanel == null || target == null || scrollRect == null)
        {
            Debug.LogWarning("Not scrolling to target; Invalid target or content");
            return;
        }

        contentPanel.anchoredPosition =
            //-(Vector2)target.localPosition;
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public static void DestroyUi(RectTransform ui)
    {
        var root = ui.root;
        Destroy(root.GetComponent<Canvas>().worldCamera.gameObject);
        Destroy(root.gameObject);
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
                                this.Text("This is double nested line 1"),
                                this.Text("This is double nested line 2"),
                                this.Button("Button", () => Debug.Log("Button Pressed")),
                                this.Toggle("Toggle", b => Debug.Log(b), out var toggle),
                                this.Slider(
                                    "Slider",
                                    0,
                                    1,
                                    false,
                                    f => f.ToString(),
                                    f => Debug.Log(f),
                                    out var slider
                                )
                            )
                    ),
                this.Text("This is main line 2"),
                this.Button("Button", () => Debug.Log("Button Pressed")),
                this.Toggle("Toggle", b => Debug.Log(b), out var act4),
                this.Slider(
                    "Slider",
                    0,
                    1,
                    false,
                    f => f.ToString(),
                    f => Debug.Log(f),
                    out var act5
                )
            );

        toggle.isOn = true;
        slider.value = 0.5f;

        //This is not required in play mode :)
        LayoutRebuilder.ForceRebuildLayoutImmediate(ui);
    }
}
