using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// 1) need to figure out how to set up the ability for characters to *remember* your dialogue choices, and react to contradictory choices in the future
    /// 2) important decisions will need to have a tag associated with them (something specific to that conversation) which can either be associated with a bool or int 
    ///     (each kind of value representing possible memories; 0 meaning not yet encountered).
    /// 3) each character needs to have a method for storing data related to player choices, stats, etc.

    // Declaring Singleton ----------------------
    private static GameManager instance;
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
    public static GameManager GetInstance()
    {
        return instance;
    }
    // ------------------------------------------

    [SerializeField] private TextAsset inkJSON; 

    public void StartStory() // this is not final---just for testing purposes
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
