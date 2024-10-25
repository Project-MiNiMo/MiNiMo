using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    private GameClient _gameClient;
    private readonly string _endpoint = "api/accounts";

    private async void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();

        // Fetch players when the game starts
        await CreateNewPlayer();

        // Fetch all players
        await FetchPlayers();
        
        // Delete a player
        await DeletePlayer(1);
    }

    private async UniTask FetchPlayers()
    {
        try
        {
            // Get all players
            var players = await _gameClient.GetAsync<List<AccountDTO>>(_endpoint);
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
        var newPlayer = new AccountDTO
        {
            Name = "New Hero",
            Level = 1,
            Experience = 0
        };

        try
        {
            var createdPlayer = await _gameClient.PostAsync<AccountDTO>(_endpoint, null);
            Debug.Log($"Created player: {createdPlayer.Name} with ID: {createdPlayer.ID}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create player: {ex.Message}");
        }
    }

    private async UniTask DeletePlayer(int playerId)
    {
        try
        {
            bool success = await _gameClient.DeleteAsync($"{_endpoint}/{playerId}");
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
