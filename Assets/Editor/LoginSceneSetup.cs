using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class LoginSceneSetup : MonoBehaviour
{
    [MenuItem("Tools/Setup Login Scene")]
    static void SetupLoginScene()
    {
        // Ensure we're in the Login scene
        Scene loginScene = EditorSceneManager.GetActiveScene();
        if (loginScene.name != "Login")
        {
            string loginScenePath = "Assets/Scenes/Login.unity";
            EditorSceneManager.OpenScene(loginScenePath);
            loginScene = EditorSceneManager.GetActiveScene();
        }
        
        // Create LoginSceneInitializer if it doesn't exist
        LoginSceneInitializer initializer = FindObjectOfType<LoginSceneInitializer>();
        if (initializer == null)
        {
            GameObject initializerGO = new GameObject("LoginSceneInitializer");
            initializer = initializerGO.AddComponent<LoginSceneInitializer>();
            
            Debug.Log("LoginSceneInitializer added to the scene. The login UI will be automatically created when you play the scene.");
        }
        else
        {
            Debug.Log("LoginSceneInitializer already exists in the scene.");
        }
        
        // Mark scene as dirty so it can be saved
        EditorSceneManager.MarkSceneDirty(loginScene);
        
        // Optional: Save the scene automatically
        EditorSceneManager.SaveScene(loginScene);
        
        Debug.Log("Login scene setup completed!");
    }
    
    [MenuItem("Tools/Create Login UI Now (Editor Preview)")]
    static void CreateLoginUIInEditor()
    {
        // Create 3D background first
        LoginSceneDesigner designer = FindObjectOfType<LoginSceneDesigner>();
        if (designer == null)
        {
            GameObject designerGO = new GameObject("LoginSceneDesigner");
            designer = designerGO.AddComponent<LoginSceneDesigner>();
            Debug.Log("3D background created.");
        }
        
        // Create UI directly in editor for preview
        LoginUIController controller = FindObjectOfType<LoginUIController>();
        if (controller == null)
        {
            GameObject uiControllerGO = new GameObject("LoginUIController");
            controller = uiControllerGO.AddComponent<LoginUIController>();
        }
        
        // Force create UI immediately
        controller.SendMessage("CreateLoginUI", SendMessageOptions.DontRequireReceiver);
        
        Debug.Log("Mystical login scene with 3D background created in editor for preview!");
    }
    
    [MenuItem("Tools/Create 3D Background Only")]
    static void Create3DBackgroundOnly()
    {
        LoginSceneDesigner designer = FindObjectOfType<LoginSceneDesigner>();
        if (designer == null)
        {
            GameObject designerGO = new GameObject("LoginSceneDesigner");
            designer = designerGO.AddComponent<LoginSceneDesigner>();
            Debug.Log("3D medieval background created!");
        }
        else
        {
            // Regenerate existing background
            designer.SendMessage("RegenerateScene", SendMessageOptions.DontRequireReceiver);
            Debug.Log("3D background regenerated!");
        }
    }
    
    [MenuItem("Tools/Open Login Scene")]
    static void OpenLoginScene()
    {
        string loginScenePath = "Assets/Scenes/Login.unity";
        EditorSceneManager.OpenScene(loginScenePath);
        Debug.Log("Login scene opened.");
    }
    
    [MenuItem("Tools/Create Login UI Prefab")]
    static void CreateLoginUIPrefab()
    {
        // Create a temporary GameObject with all login UI components
        GameObject loginUIPrefab = new GameObject("LoginUI_Complete");
        
        // Add all necessary components
        loginUIPrefab.AddComponent<LoginUIController>();
        loginUIPrefab.AddComponent<FontApplier>();
        loginUIPrefab.AddComponent<LoginSceneInitializer>();
        
        // Save as prefab
        string prefabPath = "Assets/Prefabs/LoginUI_Complete.prefab";
        PrefabUtility.SaveAsPrefabAsset(loginUIPrefab, prefabPath);
        
        // Clean up temporary object
        DestroyImmediate(loginUIPrefab);
        
        Debug.Log($"Complete Login UI prefab created at: {prefabPath}");
    }
}