using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;
    public InputField loadIndexInput;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    void SaveGame()
    {
        playerController.SaveGame();
        Debug.Log("Game Saved");
    }

    void LoadGame()
    {
        if (int.TryParse(loadIndexInput.text, out int index))
        {
            playerController.LoadGame(index);
            Debug.Log("Game Loaded from index: " + index);
        }
        else
        {
            Debug.LogWarning("Invalid load index");
        }
    }
}