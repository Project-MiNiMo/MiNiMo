using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using UnityEditor;
using UnityEngine;

public class AccountAssetViewer : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Tools/Account Asset Viewer")]
    public static void ShowWindow()
    {
        GetWindow<AccountAssetViewer>("Account Asset Viewer");
    }
    
    private void OnGUI()
    {
        var gameClient = App.Services.GetRequiredService<GameClient>();
        if (gameClient == null)
        {
            EditorGUILayout.HelpBox("GameClient is not registered in the service container.", MessageType.Error);
            return;
        }
        
        // 스크롤뷰 시작
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // AccountInfo 표시
        var account = gameClient.AccountInfo;
        if (account == null)
        {
            EditorGUILayout.HelpBox("Account is not loaded.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.LabelField("Account Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"ID: {account.ID}");
            EditorGUILayout.LabelField($"Username: {account.Nickname}");
            EditorGUILayout.LabelField($"Level: {account.Level}");
            EditorGUILayout.LabelField($"Exp: {account.Experience}");
            
            // 커런시 표시
            EditorGUILayout.LabelField("Currency", EditorStyles.boldLabel); // Star, BlueStar, Heart, HPI
            EditorGUILayout.LabelField($"Star: {account.Currency.Star}");
            EditorGUILayout.LabelField($"BlueStar: {account.Currency.BlueStar}");
            EditorGUILayout.LabelField($"Heart: {account.Currency.Heart}");
            EditorGUILayout.LabelField($"HPI: {account.Currency.HPI}");
            
            // 설치된 빌딩
            EditorGUILayout.LabelField("Buildings", EditorStyles.boldLabel);
            // 토글
            foreach (var building in account.Buildings)
            {
                EditorGUILayout.LabelField($"ID: {building.Id}");
                EditorGUILayout.LabelField($"Type: {building.BuildingType}");
                EditorGUILayout.LabelField($"ActivatedAt: {building.ActivatedAt}");
                EditorGUILayout.LabelField($"Position: {building.Position}");
                EditorGUILayout.LabelField($"ProduceStatus: {building.ProduceStatus}");
                EditorGUILayout.LabelField($"Recipes: {building.Recipes}");
                EditorGUILayout.LabelField($"ProduceStartAt: {building.ProduceStartAt}");
                EditorGUILayout.LabelField($"ProduceEndAt: {building.ProduceEndAt}");
            }
            
            // 빌딩 정보
            EditorGUILayout.LabelField("Building Info", EditorStyles.boldLabel);
            // 토글
            foreach (var buildingInfo in account.BuildingInfos)
            {
                EditorGUILayout.LabelField($"Type: {buildingInfo.BuildingType}");
                EditorGUILayout.LabelField($"OwnCount: {buildingInfo.OwnCount}");
                EditorGUILayout.LabelField($"MaxCount: {buildingInfo.MaxCount}");
                EditorGUILayout.LabelField($"InstallCount: {buildingInfo.InstallCount}");
                EditorGUILayout.LabelField($"ProduceSlotCount: {buildingInfo.ProduceSlotCount}");
            }
            
            // 아이템
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
            // 토글
            foreach (var item in account.Items)
            {
                EditorGUILayout.LabelField($"ItemType: {item.ItemType}");
                EditorGUILayout.LabelField($"Count: {item.Count}");
            }
        }
        
        // 스크롤뷰 끝
        EditorGUILayout.EndScrollView();
    }
}
