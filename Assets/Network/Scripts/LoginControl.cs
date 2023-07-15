using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

class LoginInput
{
    public string email;
    public string password;
}

class LoginSuccess
{
    public string access_token;
}

public class LoginControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text status;

    void Start()
    {
    }

    bool Validate()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            SetStatus("Chưa nhập username");
            return false;
        }

        if (string.IsNullOrEmpty(passwordInput.text))
        {
            SetStatus("Chưa nhập password");
            return false;
        }

        return true;
    }

    public void SubmitLogin()
    {
        if (!Validate()) return;
        Login(usernameInput.text, passwordInput.text);
    }

    void Login(string username, string password)
    {
        Debug.Log($"Đăng nhập với {username} - {password}");
        StartCoroutine(LoginCoroutine(username, password));
    }

    void SetStatus(string content)
    {
        status.SetText(content);
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        // Method: POST
        // Url: "http://127.0.0.1:3000/auth/login"
        // Data: username: string, password: string

        string url = "http://127.0.0.1:3000/auth/login";
        var data = new LoginInput()
        {
            email = username,
            password = password
        };
        
        using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(data), "application/json"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginSuccess>(request.downloadHandler.text);
                ApiManager.Instance.Token = response.access_token;
                SceneManager.LoadScene("Profile");
            }
            else if (request.downloadHandler!=null)
            {
                var response = JsonUtility.FromJson<LoginResponseError>(request.downloadHandler.text);
                Debug.Log($"{response.statusCode} - {response.error} - {response.message}");
                SetStatus(response.message);
            }
        }
    }
}
