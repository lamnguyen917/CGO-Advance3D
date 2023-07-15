using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

class GetProfileSuccess
{
    public string fullName;
    public string avatar;
}

public class ProfileControl : MonoBehaviour
{
    [SerializeField] private TMP_Text status;
    [SerializeField] private TMP_Text fullName;
    [SerializeField] private Image avatar;

    void Start()
    {
        Debug.Log(ApiManager.Instance.Token);
        GetUserProfile();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GetUserProfile()
    {
        StartCoroutine(GetProfileCoroutine());
    }

    IEnumerator GetProfileCoroutine()
    {
        string url = "http://127.0.0.1:3000/user";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiManager.Instance.Token);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("SUCESS" + request.downloadHandler.text);
                var response = JsonUtility.FromJson<GetProfileSuccess>(request.downloadHandler.text);
                LoadProfile(response);
            }
            else if (request.downloadHandler != null)
            {
                var response = JsonUtility.FromJson<LoginResponseError>(request.downloadHandler.text);
                Debug.Log($"{response.statusCode} - {response.error} - {response.message}");
                SetStatus(response.message);
            }
        }
    }

    void SetStatus(string content)
    {
        status.SetText(content);
    }

    void LoadProfile(GetProfileSuccess data)
    {
        fullName.text = data.fullName;
        StartCoroutine(LoadAvatar(data.avatar));
    }

    IEnumerator LoadAvatar(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiManager.Instance.Token);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var data = request.downloadHandler.data;
                var tex = new Texture2D(512, 512);
                tex.LoadImage(data);
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                    new Vector2(tex.width / 2, tex.height / 2));
                avatar.sprite = sprite;
            }
            else if (request.downloadHandler != null)
            {
                var response = JsonUtility.FromJson<LoginResponseError>(request.downloadHandler.text);
                Debug.Log($"{response.statusCode} - {response.error} - {response.message}");
                SetStatus(response.message);
            }
        }
    }
}
