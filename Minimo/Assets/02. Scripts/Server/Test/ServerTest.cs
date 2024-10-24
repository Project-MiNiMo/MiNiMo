using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MinimoShared;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    private GameClient _gameClient;

    private async void Start()
    {
        // Initialize the GameClient with the base URL of your API
        _gameClient = new GameClient("http://localhost:5093");

        // Fetch players when the game starts
        await CreateNewPlayer();

        // Fetch all players
        await FetchPlayers();
        
        // Update a player
        await UpdatePlayer(1);
        
        // Delete a player
        await DeletePlayer(1);
    }

    private async UniTask FetchPlayers()
    {
        try
        {
            // Get all players
            var players = await _gameClient.GetPlayersAsync();
            foreach (var player in players)
            {
                Debug.Log($"Player: {player.Name}, Level: {player.Level}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch players: {ex.Message}");
        }
    }

    private async UniTask CreateNewPlayer()
    {
        var newPlayer = new PlayerDTO
        {
            Name = "New Hero",
            Level = 1,
            Experience = 0
        };

        try
        {
            var createdPlayer = await _gameClient.CreatePlayerAsync(newPlayer);
            Debug.Log($"Created player: {createdPlayer.Name} with ID: {createdPlayer.ID}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create player: {ex.Message}");
        }
    }

    private async UniTask UpdatePlayer(int playerId)
    {
        var updatedPlayer = new PlayerDTO
        {
            ID = playerId,
            Name = "Updated Hero",
            Level = 10,
            Experience = 500
        };

        try
        {
            var player = await _gameClient.UpdatePlayerAsync(playerId, updatedPlayer);
            Debug.Log($"Updated player: {player.Name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update player: {ex.Message}");
        }
    }

    private async UniTask DeletePlayer(int playerId)
    {
        try
        {
            bool success = await _gameClient.DeletePlayerAsync(playerId);
            if (success)
            {
                Debug.Log("Player deleted successfully");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to delete player: {ex.Message}");
        }
    }
}
