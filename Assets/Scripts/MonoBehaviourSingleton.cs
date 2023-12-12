using UnityEngine;

/// <summary>
/// The MonoBehaviourSingleton class is a generic class that implements the Singleton pattern for components in Unity.
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public bool dontDestroyOnLoad = false;
    public static T Get()
    {
        return instance;
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
