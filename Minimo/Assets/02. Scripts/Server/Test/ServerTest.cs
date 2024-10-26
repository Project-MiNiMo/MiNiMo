using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class AccountTest : MonoBehaviour
{
    private GameClient _gameClient;
    private readonly string _endpoint = "api/accounts";

    private async void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();

        // Fetch all accounts
        await FetchAccounts();
    }

    private async UniTask FetchAccounts()
    {
        try
        {
            // Get all accounts
            var accounts = await _gameClient.GetAsync<List<AccountDTO>>(_endpoint);
            foreach (var account in accounts)
            {
                Debug.Log($"account: {account.Nickname}, Level: {account.Level}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch accounts: {ex.Message}");
        }
    }
}
