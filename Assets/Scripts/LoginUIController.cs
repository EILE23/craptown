using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginUIController : MonoBehaviour
{
    [Header("UI Layout Settings")]
    public Vector2 panelSize = new Vector2(400, 500);
    public float elementSpacing = 20f;
    public float inputFieldHeight = 45f;
    public float buttonHeight = 50f;
    
    [Header("Generated UI Elements")]
    public GameObject loginPanel;
    public TMP_Text titleText;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text statusText;
    public Image backgroundImage;
    
    void Awake()
    {
        CreateLoginUI();
    }
    
    void CreateLoginUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("LoginCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();
            
            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }
        
        // Create background
        CreateBackground(canvas.transform);
        
        // Create login panel
        CreateLoginPanel(canvas.transform);
        
        // Add LoginManager component
        var loginManager = canvas.gameObject.GetComponent<LoginManager>();
        if (loginManager == null)
        {
            loginManager = canvas.gameObject.AddComponent<LoginManager>();
        }
        
        // Assign references to LoginManager
        AssignReferencesToLoginManager(loginManager);
        
        // Apply medieval style
        var styleComponent = canvas.gameObject.GetComponent<MedievalUIStyle>();
        if (styleComponent == null)
        {
            styleComponent = canvas.gameObject.AddComponent<MedievalUIStyle>();
        }
        
        AssignStylesToComponent(styleComponent);
    }
    
    void CreateBackground(Transform parent)
    {
        GameObject bgGO = new GameObject("Background");
        backgroundImage = bgGO.AddComponent<Image>();
        
        // Use Unity's default UI sprite to avoid pink rendering
        backgroundImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Background.psd");
        if (backgroundImage.sprite == null)
        {
            // Create a simple white sprite if default is not available
            Texture2D whiteTexture = new Texture2D(1, 1);
            whiteTexture.SetPixel(0, 0, Color.white);
            whiteTexture.Apply();
            backgroundImage.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        }
        
        backgroundImage.color = new Color(0.1f, 0.08f, 0.06f, 0.3f); // More transparent to show 3D background
        backgroundImage.type = Image.Type.Sliced;
        
        RectTransform bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.SetParent(parent, false);
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        bgRect.anchoredPosition = Vector2.zero;
    }
    
    void CreateLoginPanel(Transform parent)
    {
        // Create main panel
        loginPanel = new GameObject("LoginPanel");
        Image panelImage = loginPanel.AddComponent<Image>();
        
        // Use Unity's default UI sprite
        panelImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        if (panelImage.sprite == null)
        {
            Texture2D panelTexture = new Texture2D(1, 1);
            panelTexture.SetPixel(0, 0, Color.white);
            panelTexture.Apply();
            panelImage.sprite = Sprite.Create(panelTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        }
        
        panelImage.color = new Color(0.2f, 0.15f, 0.1f, 0.9f); // Slightly more opaque for readability
        panelImage.type = Image.Type.Sliced;
        
        RectTransform panelRect = loginPanel.GetComponent<RectTransform>();
        panelRect.SetParent(parent, false);
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = panelSize;
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add vertical layout group
        VerticalLayoutGroup layoutGroup = loginPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = elementSpacing;
        layoutGroup.padding = new RectOffset(40, 40, 40, 40);
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = true;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = true;
        
        // Create UI elements
        CreateTitleText();
        CreateInputField("UsernameField", "사용자 이름", out usernameField);
        CreateInputField("PasswordField", "비밀번호", out passwordField, true);
        CreateButton("LoginButton", "로그인", out loginButton);
        CreateButton("RegisterButton", "회원가입", out registerButton);
        CreateStatusText();
        
        // Set password field
        passwordField.contentType = TMP_InputField.ContentType.Password;
    }
    
    void CreateTitleText()
    {
        GameObject titleGO = new GameObject("TitleText");
        titleText = titleGO.AddComponent<TextMeshProUGUI>();
        titleText.text = "중세 마을에 오신 것을 환영합니다";
        titleText.fontSize = 28;
        titleText.color = new Color(0.8f, 0.7f, 0.5f, 1f);
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.SetParent(loginPanel.transform, false);
        
        LayoutElement titleLayout = titleGO.AddComponent<LayoutElement>();
        titleLayout.preferredHeight = 60;
    }
    
    void CreateInputField(string name, string placeholderText, out TMP_InputField inputField, bool isPassword = false)
    {
        GameObject fieldGO = new GameObject(name);
        Image fieldBG = fieldGO.AddComponent<Image>();
        
        // Use Unity's default input field background
        fieldBG.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/InputFieldBackground.psd");
        if (fieldBG.sprite == null)
        {
            Texture2D fieldTexture = new Texture2D(1, 1);
            fieldTexture.SetPixel(0, 0, Color.white);
            fieldTexture.Apply();
            fieldBG.sprite = Sprite.Create(fieldTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        }
        
        fieldBG.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        fieldBG.type = Image.Type.Sliced;
        
        inputField = fieldGO.AddComponent<TMP_InputField>();
        
        // Create text component
        GameObject textGO = new GameObject("Text");
        TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();
        textComponent.text = "";
        textComponent.fontSize = 16;
        textComponent.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.SetParent(fieldGO.transform, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.offsetMin = new Vector2(10, 6);
        textRect.offsetMax = new Vector2(-10, -7);
        
        // Create placeholder
        GameObject placeholderGO = new GameObject("Placeholder");
        TextMeshProUGUI placeholderComponent = placeholderGO.AddComponent<TextMeshProUGUI>();
        placeholderComponent.text = placeholderText;
        placeholderComponent.fontSize = 16;
        placeholderComponent.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        placeholderComponent.fontStyle = FontStyles.Italic;
        
        RectTransform placeholderRect = placeholderGO.GetComponent<RectTransform>();
        placeholderRect.SetParent(fieldGO.transform, false);
        placeholderRect.anchorMin = Vector2.zero;
        placeholderRect.anchorMax = Vector2.one;
        placeholderRect.sizeDelta = Vector2.zero;
        placeholderRect.offsetMin = new Vector2(10, 6);
        placeholderRect.offsetMax = new Vector2(-10, -7);
        
        // Setup input field
        inputField.textComponent = textComponent;
        inputField.placeholder = placeholderComponent;
        inputField.targetGraphic = fieldBG;
        
        if (isPassword)
        {
            inputField.contentType = TMP_InputField.ContentType.Password;
        }
        
        RectTransform fieldRect = fieldGO.GetComponent<RectTransform>();
        fieldRect.SetParent(loginPanel.transform, false);
        
        LayoutElement fieldLayout = fieldGO.AddComponent<LayoutElement>();
        fieldLayout.preferredHeight = inputFieldHeight;
    }
    
    void CreateButton(string name, string text, out Button button)
    {
        GameObject buttonGO = new GameObject(name);
        Image buttonImage = buttonGO.AddComponent<Image>();
        
        // Use Unity's default button sprite
        buttonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Button.psd");
        if (buttonImage.sprite == null)
        {
            Texture2D buttonTexture = new Texture2D(1, 1);
            buttonTexture.SetPixel(0, 0, Color.white);
            buttonTexture.Apply();
            buttonImage.sprite = Sprite.Create(buttonTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        }
        
        buttonImage.color = new Color(0.4f, 0.3f, 0.2f, 0.9f);
        buttonImage.type = Image.Type.Sliced;
        
        button = buttonGO.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        
        // Create button text
        GameObject textGO = new GameObject("Text");
        TextMeshProUGUI buttonText = textGO.AddComponent<TextMeshProUGUI>();
        buttonText.text = text;
        buttonText.fontSize = 18;
        buttonText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.fontStyle = FontStyles.Bold;
        
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.SetParent(buttonGO.transform, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        // Setup button colors
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.4f, 0.3f, 0.2f, 0.9f);
        colors.highlightedColor = new Color(0.6f, 0.4f, 0.3f, 0.9f);
        colors.pressedColor = new Color(0.3f, 0.2f, 0.1f, 0.9f);
        colors.selectedColor = new Color(0.6f, 0.4f, 0.3f, 0.9f);
        button.colors = colors;
        
        RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.SetParent(loginPanel.transform, false);
        
        LayoutElement buttonLayout = buttonGO.AddComponent<LayoutElement>();
        buttonLayout.preferredHeight = buttonHeight;
    }
    
    void CreateStatusText()
    {
        GameObject statusGO = new GameObject("StatusText");
        statusText = statusGO.AddComponent<TextMeshProUGUI>();
        statusText.text = "";
        statusText.fontSize = 14;
        statusText.color = Color.red;
        statusText.alignment = TextAlignmentOptions.Center;
        
        RectTransform statusRect = statusGO.GetComponent<RectTransform>();
        statusRect.SetParent(loginPanel.transform, false);
        
        LayoutElement statusLayout = statusGO.AddComponent<LayoutElement>();
        statusLayout.preferredHeight = 30;
    }
    
    void AssignReferencesToLoginManager(LoginManager loginManager)
    {
        loginManager.usernameField = usernameField;
        loginManager.passwordField = passwordField;
        loginManager.loginButton = loginButton;
        loginManager.registerButton = registerButton;
        loginManager.titleText = titleText;
        loginManager.statusText = statusText;
        loginManager.backgroundImage = backgroundImage;
    }
    
    void AssignStylesToComponent(MedievalUIStyle styleComponent)
    {
        styleComponent.titleTexts = new TMP_Text[] { titleText };
        styleComponent.normalTexts = new TMP_Text[] { statusText };
        styleComponent.buttons = new Button[] { loginButton, registerButton };
        styleComponent.panels = new Image[] { loginPanel.GetComponent<Image>() };
        styleComponent.inputFields = new TMP_InputField[] { usernameField, passwordField };
    }
}