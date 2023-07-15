using UnityEngine;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance;
    public string Token;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
