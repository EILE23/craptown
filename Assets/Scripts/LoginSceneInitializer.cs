using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneInitializer : MonoBehaviour
{
    [Header("Setup Options")]
    public bool autoCreateUI = true;
    public bool useBackgroundTexture = true;
    public bool enableEventSystem = true;
    
    [Header("Background Settings")]
    public Texture2D customBackgroundTexture;
    public Color backgroundColor = new Color(0.1f, 0.08f, 0.06f, 0.9f);
    public bool create3DBackground = true;
    
    void Start()
    {
        if (autoCreateUI)
        {
            SetupLoginScene();
        }
    }
    
    void SetupLoginScene()
    {
        // Ensure EventSystem exists
        if (enableEventSystem && UnityEngine.EventSystems.EventSystem.current == null)
        {
            CreateEventSystem();
        }
        
        // Create 3D background scene
        if (create3DBackground && FindObjectOfType<LoginSceneDesigner>() == null)
        {
            Create3DBackground();
        }
        
        // Create UI if it doesn't exist
        if (FindObjectOfType<LoginUIController>() == null)
        {
            CreateLoginUI();
        }
        
        // Apply background texture from medieval assets if available
        if (useBackgroundTexture && customBackgroundTexture == null)
        {
            LoadMedievalBackground();
        }
        
        // Apply fonts
        ApplyFonts();
    }
    
    void CreateEventSystem()
    {
        GameObject eventSystemGO = new GameObject("EventSystem");
        eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }
    
    void Create3DBackground()
    {
        GameObject backgroundGO = new GameObject("LoginSceneBackground");
        backgroundGO.AddComponent<LoginSceneDesigner>();
    }
    
    void CreateLoginUI()
    {
        GameObject uiControllerGO = new GameObject("LoginUIController");
        uiControllerGO.AddComponent<LoginUIController>();
    }
    
    void LoadMedievalBackground()
    {
        // Try to load the medieval town texture
        string[] texturePaths = {
            "FantasyMedievalTown_LITE/Textures/CartoonTownLite_Texture_01",
            "CartoonTownLite_Texture_01",
            "Textures/CartoonTownLite_Texture_01"
        };
        
        foreach (string path in texturePaths)
        {
            Texture2D texture = Resources.Load<Texture2D>(path);
            if (texture != null)
            {
                customBackgroundTexture = texture;
                Debug.Log($"Loaded medieval background texture: {texture.name}");
                break;
            }
        }
        
        if (customBackgroundTexture == null)
        {
            Debug.Log("Medieval background texture not found in Resources. Using solid color background.");
        }
    }
    
    void ApplyFonts()
    {
        // Apply Korean font after UI is created
        Invoke("DelayedFontApplication", 0.1f);
    }
    
    void DelayedFontApplication()
    {
        FontApplier fontApplier = FindObjectOfType<FontApplier>();
        if (fontApplier == null)
        {
            GameObject fontGO = new GameObject("FontApplier");
            fontApplier = fontGO.AddComponent<FontApplier>();
        }
        
        fontApplier.ApplyFontToLoginUI();
    }
    
    // Method to manually trigger scene setup (useful for testing)
    [ContextMenu("Setup Login Scene")]
    public void ManualSetup()
    {
        SetupLoginScene();
    }
    
    // Method to reload the scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Method to load main scene after successful login
    public void LoadMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}