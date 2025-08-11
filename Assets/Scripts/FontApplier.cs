using UnityEngine;
using TMPro;

public class FontApplier : MonoBehaviour
{
    [Header("Font Assets")]
    public TMP_FontAsset koreanFont;
    public TMP_FontAsset defaultFont;
    
    [Header("Auto-apply Settings")]
    public bool applyOnStart = true;
    public bool findFontAutomatically = true;
    
    void Start()
    {
        if (findFontAutomatically)
        {
            LoadKoreanFont();
        }
        
        if (applyOnStart)
        {
            ApplyFontToAllTexts();
        }
    }
    
    void LoadKoreanFont()
    {
        // Try to load the Noto Sans KR SDF font
        koreanFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/NotoSansKR-VariableFont_wght SDF");
        
        if (koreanFont == null)
        {
            // Alternative path
            string[] fontPaths = {
                "NotoSansKR-VariableFont_wght SDF",
                "Fonts/NotoSansKR-VariableFont_wght SDF",
                "Assets/Fonts/NotoSansKR-VariableFont_wght SDF"
            };
            
            foreach (string path in fontPaths)
            {
                koreanFont = Resources.Load<TMP_FontAsset>(path);
                if (koreanFont != null) break;
            }
        }
        
        if (koreanFont != null)
        {
            Debug.Log($"Korean font loaded successfully: {koreanFont.name}");
        }
        else
        {
            Debug.LogWarning("Korean font not found. Using default font.");
        }
    }
    
    public void ApplyFontToAllTexts()
    {
        if (koreanFont == null) return;
        
        // Find all TMP_Text components in the scene
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);
        
        foreach (TMP_Text text in allTexts)
        {
            ApplyFontToText(text);
        }
        
        Debug.Log($"Applied Korean font to {allTexts.Length} text components.");
    }
    
    public void ApplyFontToText(TMP_Text textComponent)
    {
        if (textComponent != null && koreanFont != null)
        {
            textComponent.font = koreanFont;
        }
    }
    
    public void ApplyFontToLoginUI()
    {
        // Specifically target login UI elements
        LoginManager loginManager = FindObjectOfType<LoginManager>();
        if (loginManager != null)
        {
            if (loginManager.titleText != null)
                ApplyFontToText(loginManager.titleText);
            
            if (loginManager.statusText != null)
                ApplyFontToText(loginManager.statusText);
            
            if (loginManager.usernameField != null)
            {
                ApplyFontToText(loginManager.usernameField.textComponent);
                if (loginManager.usernameField.placeholder != null)
                {
                    var placeholder = loginManager.usernameField.placeholder.GetComponent<TMP_Text>();
                    if (placeholder != null) ApplyFontToText(placeholder);
                }
            }
            
            if (loginManager.passwordField != null)
            {
                ApplyFontToText(loginManager.passwordField.textComponent);
                if (loginManager.passwordField.placeholder != null)
                {
                    var placeholder = loginManager.passwordField.placeholder.GetComponent<TMP_Text>();
                    if (placeholder != null) ApplyFontToText(placeholder);
                }
            }
            
            if (loginManager.loginButton != null)
            {
                var buttonText = loginManager.loginButton.GetComponentInChildren<TMP_Text>();
                if (buttonText != null) ApplyFontToText(buttonText);
            }
            
            if (loginManager.registerButton != null)
            {
                var buttonText = loginManager.registerButton.GetComponentInChildren<TMP_Text>();
                if (buttonText != null) ApplyFontToText(buttonText);
            }
        }
    }
    
    // Method to manually assign font reference
    public void SetKoreanFont(TMP_FontAsset font)
    {
        koreanFont = font;
        ApplyFontToAllTexts();
    }
    
    // Helper method to check if text contains Korean characters
    public static bool ContainsKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        
        foreach (char c in text)
        {
            if (IsKoreanCharacter(c))
                return true;
        }
        return false;
    }
    
    public static bool IsKoreanCharacter(char c)
    {
        // Korean character ranges
        return (c >= 0xAC00 && c <= 0xD7AF) ||  // Hangul syllables
               (c >= 0x1100 && c <= 0x11FF) ||  // Hangul Jamo
               (c >= 0x3130 && c <= 0x318F) ||  // Hangul Compatibility Jamo
               (c >= 0xA960 && c <= 0xA97F) ||  // Hangul Jamo Extended-A
               (c >= 0xD7B0 && c <= 0xD7FF);    // Hangul Jamo Extended-B
    }
}