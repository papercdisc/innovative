using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// resources: https://www.youtube.com/watch?v=vY0Sk93YUhA&list=PL3viUl9h9k78KsDxXoAzgQ1yRjhm7p8kl&index=3
/// </summary>

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
}
