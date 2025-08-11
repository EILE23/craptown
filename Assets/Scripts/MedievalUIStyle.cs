using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MedievalUIColors
{
    [Header("Text Colors")]
    public Color titleColor = new Color(0.8f, 0.7f, 0.5f, 1f);
    public Color normalTextColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    public Color placeholderColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    
    [Header("Button Colors")]
    public Color buttonNormalColor = new Color(0.4f, 0.3f, 0.2f, 0.9f);
    public Color buttonHighlightColor = new Color(0.6f, 0.4f, 0.3f, 0.9f);
    public Color buttonPressedColor = new Color(0.3f, 0.2f, 0.1f, 0.9f);
    
    [Header("Panel Colors")]
    public Color panelColor = new Color(0.2f, 0.15f, 0.1f, 0.8f);
    public Color inputFieldColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
}

public class MedievalUIStyle : MonoBehaviour
{
    [Header("Style Settings")]
    public MedievalUIColors colors = new MedievalUIColors();
    
    [Header("Components to Style")]
    public TMP_Text[] titleTexts;
    public TMP_Text[] normalTexts;
    public Button[] buttons;
    public Image[] panels;
    public TMP_InputField[] inputFields;
    
    void Start()
    {
        ApplyMedievalStyle();
    }
    
    public void ApplyMedievalStyle()
    {
        // Style title texts
        foreach (var titleText in titleTexts)
        {
            if (titleText != null)
            {
                titleText.color = colors.titleColor;
                titleText.fontSize = Mathf.Max(titleText.fontSize, 32);
                titleText.fontStyle = FontStyles.Bold;
            }
        }
        
        // Style normal texts
        foreach (var normalText in normalTexts)
        {
            if (normalText != null)
            {
                normalText.color = colors.normalTextColor;
            }
        }
        
        // Style buttons
        foreach (var button in buttons)
        {
            if (button != null)
            {
                var colorBlock = button.colors;
                colorBlock.normalColor = colors.buttonNormalColor;
                colorBlock.highlightedColor = colors.buttonHighlightColor;
                colorBlock.pressedColor = colors.buttonPressedColor;
                colorBlock.selectedColor = colors.buttonHighlightColor;
                button.colors = colorBlock;
                
                // Add border effect
                var buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = colors.buttonNormalColor;
                }
                
                // Style button text
                var buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.color = colors.normalTextColor;
                    buttonText.fontSize = Mathf.Max(buttonText.fontSize, 18);
                }
            }
        }
        
        // Style panels
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.color = colors.panelColor;
            }
        }
        
        // Style input fields
        foreach (var inputField in inputFields)
        {
            if (inputField != null)
            {
                var targetGraphic = inputField.targetGraphic;
                if (targetGraphic != null)
                {
                    targetGraphic.color = colors.inputFieldColor;
                }
                
                inputField.textComponent.color = colors.normalTextColor;
                
                if (inputField.placeholder != null)
                {
                    var placeholderText = inputField.placeholder.GetComponent<TMP_Text>();
                    if (placeholderText != null)
                    {
                        placeholderText.color = colors.placeholderColor;
                    }
                }
            }
        }
    }
    
    public void SetBackgroundTexture(Texture2D texture)
    {
        var backgroundImage = GetComponent<Image>();
        if (backgroundImage != null && texture != null)
        {
            backgroundImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            backgroundImage.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }
}