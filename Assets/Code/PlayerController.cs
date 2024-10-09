using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameStateManager gameStateManager;

    void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    public void SaveGame()
    {
        Vector3 position = transform.position;
        Color color = GetComponent<Renderer>().material.color;
        gameStateManager.SaveState(position, color);
    }

    public void LoadGame(int saveIndex)
    {
        gameStateManager.LoadState(saveIndex);
    }

    // ??????????????????????????????????????? ????????????? ???
}