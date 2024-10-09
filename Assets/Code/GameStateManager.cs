using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameStateMemento
{
    public float[] PlayerPosition;
    public float[] PlayerColor;

    public GameStateMemento(Vector3 position, Color color)
    {
        PlayerPosition = new float[3] { position.x, position.y, position.z };
        PlayerColor = new float[4] { color.r, color.g, color.b, color.a };
    }
}

public class GameStateManager : MonoBehaviour
{
    private List<GameStateMemento> savedStates = new List<GameStateMemento>();
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
        savedStates = new List<GameStateMemento>();
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }
    public void SaveState(Vector3 position, Color color)
    {
        GameStateMemento memento = new GameStateMemento(position, color);
        savedStates.Add(memento);
        SaveToDisk();
    }

    void Start()
    {
        LoadFromDisk();
     

    }

    public void LoadState(int index)
    {
        if (index >= 0 && index < savedStates.Count)
        {
            GameStateMemento memento = savedStates[index];
            ApplyState(memento);
        }
        else
        {
            Debug.LogWarning("Invalid save state index.");
        }
    }

    private void ApplyState(GameStateMemento memento)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(memento.PlayerPosition[0], memento.PlayerPosition[1], memento.PlayerPosition[2]);
            Renderer renderer = player.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(memento.PlayerColor[0], memento.PlayerColor[1], memento.PlayerColor[2], memento.PlayerColor[3]);
            }
        }
    }

    private void SaveToDisk()
    {
        if (savedStates != null && savedStates.Count > 0)
        {
            string json = JsonUtility.ToJson(new Wrapper { savedStates = this.savedStates });
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved successfully.");
        }
        else
        {
            Debug.Log("No states to save.");
        }
    }

    private void LoadFromDisk()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            if (!string.IsNullOrEmpty(json))
            {
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
                savedStates = wrapper.savedStates;
            }
            else
            {
                Debug.Log("Save file is empty. Starting with a new save.");
                savedStates = new List<GameStateMemento>();
            }
        }
        else
        {
            Debug.Log("No save file found. Starting with a new save.");
            savedStates = new List<GameStateMemento>();
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<GameStateMemento> savedStates;
    }

    void OnEnable()
    {
        LoadFromDisk();
    }

    void OnDisable()
    {
        SaveToDisk();
    }
}