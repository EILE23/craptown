using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoginSceneDesigner : MonoBehaviour
{
    [Header("Scene Layout")]
    public GameObject medievalHousePrefab;
    public GameObject[] decorativePrefabs;
    
    [Header("Atmosphere Settings")]
    public Color fogColor = new Color(0.3f, 0.4f, 0.6f, 1f);
    public float fogDensity = 0.02f;
    public Color ambientColor = new Color(0.2f, 0.3f, 0.5f, 1f);
    
    [Header("Lighting")]
    public Color mainLightColor = new Color(0.8f, 0.9f, 1f, 1f);
    public float mainLightIntensity = 1.2f;
    public Vector3 mainLightRotation = new Vector3(45f, -30f, 0f);
    
    [Header("Camera Settings")]
    public Vector3 cameraPosition = new Vector3(0, 5, -10);
    public Vector3 cameraRotation = new Vector3(10, 0, 0);
    
    void Start()
    {
        CreateMysticalLoginScene();
    }
    
    void CreateMysticalLoginScene()
    {
        SetupCamera();
        SetupLighting();
        SetupAtmosphere();
        CreateMedievalBackground();
        SetupPostProcessing();
    }
    
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraGO = new GameObject("Main Camera");
            mainCamera = cameraGO.AddComponent<Camera>();
            cameraGO.tag = "MainCamera";
        }
        
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.eulerAngles = cameraRotation;
        mainCamera.backgroundColor = new Color(0.1f, 0.15f, 0.25f, 1f);
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        
        // Add URP camera component
        var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        if (cameraData == null)
        {
            cameraData = mainCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
        }
    }
    
    void SetupLighting()
    {
        // Create main directional light (moonlight effect)
        GameObject lightGO = new GameObject("Directional Light");
        Light mainLight = lightGO.AddComponent<Light>();
        mainLight.type = LightType.Directional;
        mainLight.color = mainLightColor;
        mainLight.intensity = mainLightIntensity;
        mainLight.shadows = LightShadows.Soft;
        lightGO.transform.eulerAngles = mainLightRotation;
        
        // Set ambient lighting
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambientColor;
        
        // Create atmospheric lighting
        CreateAtmosphericLights();
    }
    
    void CreateAtmosphericLights()
    {
        // Create some dim area lights for atmosphere
        for (int i = 0; i < 3; i++)
        {
            GameObject lightGO = new GameObject($"Atmospheric Light {i + 1}");
            Light light = lightGO.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(0.6f, 0.8f, 1f, 1f);
            light.intensity = 0.5f;
            light.range = 15f;
            
            // Random positions around the scene
            float angle = (i * 120f) * Mathf.Deg2Rad;
            float distance = 12f + Random.Range(-3f, 3f);
            lightGO.transform.position = new Vector3(
                Mathf.Sin(angle) * distance,
                3f + Random.Range(-1f, 2f),
                Mathf.Cos(angle) * distance
            );
        }
    }
    
    void SetupAtmosphere()
    {
        // Enable fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = fogDensity;
        
        // Set skybox to a dark, moody one
        RenderSettings.skybox = null;
    }
    
    void CreateMedievalBackground()
    {
        // Create ground plane
        CreateGroundPlane();
        
        // Load and place medieval assets
        LoadMedievalPrefabs();
        
        // Create the main building in the background
        CreateMainBuilding();
        
        // Add atmospheric elements
        CreateAtmosphericElements();
    }
    
    void CreateGroundPlane()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(5, 1, 5);
        
        // Create a dark ground material
        Material groundMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        groundMaterial.color = new Color(0.1f, 0.15f, 0.1f, 1f);
        groundMaterial.SetFloat("_Smoothness", 0.1f);
        ground.GetComponent<Renderer>().material = groundMaterial;
    }
    
    void LoadMedievalPrefabs()
    {
        // Try to load medieval house prefabs
        string[] prefabPaths = {
            "FantasyMedievalTown_LITE/Prefabs/House_01_LITE",
            "House_01_LITE",
            "HouseBase_03_LITE",
            "HouseWall_02_LITE"
        };
        
        GameObject housePrefab = null;
        foreach (string path in prefabPaths)
        {
            housePrefab = Resources.Load<GameObject>(path);
            if (housePrefab != null)
            {
                Debug.Log($"Loaded medieval house: {path}");
                break;
            }
        }
        
        if (housePrefab != null)
        {
            medievalHousePrefab = housePrefab;
        }
    }
    
    void CreateMainBuilding()
    {
        if (medievalHousePrefab != null)
        {
            // Create main building in the background
            GameObject mainBuilding = Instantiate(medievalHousePrefab);
            mainBuilding.name = "Main Medieval Building";
            mainBuilding.transform.position = new Vector3(0, 0, 15);
            mainBuilding.transform.localScale = Vector3.one * 2f;
        }
        else
        {
            // Create a simple building substitute
            CreateSimpleBuilding();
        }
    }
    
    void CreateSimpleBuilding()
    {
        // Create a simple building using primitives
        GameObject building = new GameObject("Simple Medieval Building");
        
        // Main structure
        GameObject main = GameObject.CreatePrimitive(PrimitiveType.Cube);
        main.transform.SetParent(building.transform);
        main.transform.localPosition = new Vector3(0, 2, 0);
        main.transform.localScale = new Vector3(8, 4, 6);
        
        // Roof
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.transform.SetParent(building.transform);
        roof.transform.localPosition = new Vector3(0, 4.5f, 0);
        roof.transform.localScale = new Vector3(9, 1, 7);
        
        // Apply dark materials
        Material buildingMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        buildingMaterial.color = new Color(0.3f, 0.25f, 0.2f, 1f);
        main.GetComponent<Renderer>().material = buildingMaterial;
        
        Material roofMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        roofMaterial.color = new Color(0.2f, 0.15f, 0.1f, 1f);
        roof.GetComponent<Renderer>().material = roofMaterial;
        
        building.transform.position = new Vector3(0, 0, 15);
    }
    
    void CreateAtmosphericElements()
    {
        // Create some trees or decorative elements
        for (int i = 0; i < 5; i++)
        {
            CreateTree(new Vector3(
                Random.Range(-20f, 20f),
                0,
                Random.Range(5f, 25f)
            ));
        }
        
        // Create fog volumes (visual effect)
        CreateFogVolumes();
    }
    
    void CreateTree(Vector3 position)
    {
        GameObject tree = new GameObject($"Tree_{position.x:F0}_{position.z:F0}");
        
        // Trunk
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.transform.SetParent(tree.transform);
        trunk.transform.localPosition = new Vector3(0, 1, 0);
        trunk.transform.localScale = new Vector3(0.5f, 2f, 0.5f);
        
        // Leaves
        GameObject leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves.transform.SetParent(tree.transform);
        leaves.transform.localPosition = new Vector3(0, 3, 0);
        leaves.transform.localScale = Vector3.one * 3f;
        
        // Apply materials
        Material trunkMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        trunkMaterial.color = new Color(0.3f, 0.2f, 0.1f, 1f);
        trunk.GetComponent<Renderer>().material = trunkMaterial;
        
        Material leavesMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        leavesMaterial.color = new Color(0.1f, 0.2f, 0.1f, 1f);
        leaves.GetComponent<Renderer>().material = leavesMaterial;
        
        tree.transform.position = position;
    }
    
    void CreateFogVolumes()
    {
        // Create particle systems for fog effect if needed
        // This is a placeholder for atmospheric particles
    }
    
    void SetupPostProcessing()
    {
        // Add post-processing volume for cinematic look
        GameObject ppVolume = new GameObject("Post Process Volume");
        Volume volume = ppVolume.AddComponent<Volume>();
        volume.isGlobal = true;
        
        // You would typically assign a post-processing profile here
        // This requires the Volume profile asset to be created
    }
    
    // Public method to regenerate scene
    [ContextMenu("Regenerate Login Scene")]
    public void RegenerateScene()
    {
        // Clear existing elements
        GameObject[] existingObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in existingObjects)
        {
            if (obj.name.Contains("Medieval") || obj.name.Contains("Tree") || obj.name.Contains("Ground"))
            {
                DestroyImmediate(obj);
            }
        }
        
        CreateMysticalLoginScene();
    }
}