using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text titleText;
    public TMP_Text statusText;
    
    [Header("Background")]
    public Image backgroundImage;
    
    void Start()
    {
        SetupUI();
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }
    
    void SetupUI()
    {
        if (titleText != null)
        {
            titleText.text = "중세 마을에 오신 것을 환영합니다";
            titleText.fontSize = 36;
            titleText.color = new Color(0.8f, 0.7f, 0.5f, 1f);
        }
        
        if (usernameField != null)
        {
            usernameField.placeholder.GetComponent<TMP_Text>().text = "사용자 이름";
        }
        
        if (passwordField != null)
        {
            passwordField.placeholder.GetComponent<TMP_Text>().text = "비밀번호";
            passwordField.contentType = TMP_InputField.ContentType.Password;
        }
        
        if (statusText != null)
        {
            statusText.text = "";
            statusText.color = Color.red;
        }
    }
    
    void OnLoginClick()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowStatus("사용자 이름과 비밀번호를 모두 입력해주세요.");
            return;
        }
        
        ShowStatus("로그인 중...");
        PerformLogin(username, password);
    }
    
    void OnRegisterClick()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowStatus("사용자 이름과 비밀번호를 모두 입력해주세요.");
            return;
        }
        
        if (password.Length < 6)
        {
            ShowStatus("비밀번호는 최소 6자 이상이어야 합니다.");
            return;
        }
        
        ShowStatus("계정 생성 중...");
        PerformRegister(username, password);
    }
    
    void PerformLogin(string username, string password)
    {
        if (username == "admin" && password == "password")
        {
            ShowStatus("로그인 성공!");
            statusText.color = Color.green;
            Invoke("LoadMainScene", 2f);
        }
        else
        {
            ShowStatus("잘못된 사용자 이름 또는 비밀번호입니다.");
        }
    }
    
    void PerformRegister(string username, string password)
    {
        ShowStatus("계정이 생성되었습니다! 로그인해주세요.");
        statusText.color = Color.green;
        ClearFields();
    }
    
    void ShowStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
    
    void ClearFields()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
    
    void LoadMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}