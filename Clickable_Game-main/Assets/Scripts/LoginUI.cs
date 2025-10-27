using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Firebase.Auth;

public class LoginUI : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject profilePanel;
    public GameObject createProfilePanel;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button signupButton;
    public Button anonymousButton;
    public Button profileButton;
    public TextMeshProUGUI errorText;

    private async Task Start()
    {
        // errorText 초기화 확인
        if (errorText == null)
        {
            Debug.LogError("errorText is not assigned in Inspector!");
            return;
        }

        SetButtonsInteractable(false);
        errorText.text = "Initializing...";

        await UniTask.WaitUntil(() => AuthManager.Instance != null && AuthManager.Instance.IsInitialized);

        loginButton.onClick.AddListener(() => OnLoginClicked().Forget());
        signupButton.onClick.AddListener(() => OnSignupClicked().Forget());
        anonymousButton.onClick.AddListener(() => OnAnonymousClicked().Forget());
        profileButton.onClick.AddListener(() => OnProfileButtonClicked().Forget());

        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public async UniTaskVoid UpdateUI()
    {
        if (AuthManager.Instance == null || !AuthManager.Instance.IsInitialized)
        {
            return;
        }

        bool isLoggedIn = AuthManager.Instance.IsLoggedIn;
        loginPanel.SetActive(!isLoggedIn);
        profileButton.gameObject.SetActive(isLoggedIn);

        // 로그인 성공 시 에러 메시지 초기화
        if (isLoggedIn && errorText != null)
        {
            errorText.text = string.Empty;
        }
    }

    public async UniTask OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // 입력 검증
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Email and password cannot be empty");
            return;
        }

        SetButtonsInteractable(false);
        ClearError(); // 새 시도 전 에러 메시지 초기화

        var (success, error) = await AuthManager.Instance.SighInWithEmailAsync(email, password);

        if (success)
        {
            Debug.Log("Login successful");
        }
        else
        {
            ShowError($"Login failed: {error}");
        }

        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public async UniTask OnSignupClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Email and password cannot be empty");
            return;
        }

        SetButtonsInteractable(false);
        ClearError();

        var (success, error) = await AuthManager.Instance.CreateUserWithEmailAsync(email, password);

        if (success)
        {
            Debug.Log("Signup successful");
        }
        else
        {
            ShowError($"Signup failed: {error}");
        }

        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public async UniTask OnAnonymousClicked()
    {
        SetButtonsInteractable(false);
        ClearError();

        var (success, error) = await AuthManager.Instance.SingInAnonymouslyAsync();

        if (success)
        {
            Debug.Log("Anonymous login successful");
        }
        else
        {
            ShowError($"Anonymous login failed: {error}");
        }

        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public void ShowError(string error)
    {
        if (errorText != null)
        {
            errorText.text = error;
            Debug.LogError($"UI Error: {error}");
        }
        else
        {
            Debug.LogError($"errorText is null! Error message: {error}");
        }
    }

    public void ClearError()
    {
        if (errorText != null)
        {
            errorText.text = string.Empty;
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        loginButton.interactable = interactable;
        signupButton.interactable = interactable;
        anonymousButton.interactable = interactable;
    }

    private async UniTaskVoid OnProfileButtonClicked()
    {
        ClearError();

        if (await ProfileManager.Instance.ProfileExistAsync())
        {
            profilePanel.SetActive(true);
        }
        else
        {
            createProfilePanel.SetActive(true);
        }
    }
}