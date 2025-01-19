using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEditor;
using UnityEngine;

public class AccountAssetViewer : EditorWindow
{
    private Vector2 scrollPosition;
    
    private bool showBuildings = true;
    private bool showBuildingInfo = true;
    private bool showItems = true;
    
    [MenuItem("Tools/Account Asset Viewer")]
    public static void ShowWindow()
    {
        GetWindow<AccountAssetViewer>("Account Asset Viewer");
    }
    
    private void OnGUI()
    {
        if(App.Services == null)
        {
            EditorGUILayout.HelpBox("Service container is not initialized.", MessageType.Error);
            return;
        }
        
        var gameClient = App.Services?.GetRequiredService<GameClient>();
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
            // Begin Area
            EditorGUILayout.LabelField("Account Info", EditorStyles.boldLabel);
            GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
            
            EditorGUILayout.LabelField($"ID: {account.ID}");
            EditorGUILayout.LabelField($"Username: {account.Nickname}");
            EditorGUILayout.LabelField($"Level: {account.Level}");
            EditorGUILayout.LabelField($"Exp: {account.Experience}");
            
            EditorGUILayout.EndVertical();
            
            // 커런시 표시
            // 전체 커런시 박스 처리
            EditorGUILayout.LabelField("Currency", EditorStyles.boldLabel); // Star, BlueStar, Heart, HPI
            GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });

            EditorGUILayout.LabelField($"Star: {account.Currency.Star}");
            EditorGUILayout.LabelField($"BlueStar: {account.Currency.BlueStar}");
            EditorGUILayout.LabelField($"Heart: {account.Currency.Heart}");
            EditorGUILayout.LabelField($"HPI: {account.Currency.HPI}");
            
            EditorGUILayout.EndVertical();
            
            // 설치된 빌딩
            EditorGUILayout.LabelField("Buildings", EditorStyles.boldLabel);
            // Fold out. 각 빌딩은 박스 처리
            foreach (var building in account.Buildings)
            {
                DisplayBuilding(building);
            }
            
            // 빌딩 정보
            EditorGUILayout.LabelField("Building Info", EditorStyles.boldLabel);
            // Fold out. 각 빌딩 정보는 박스 처리
            foreach (var buildingInfo in account.BuildingInfos)
            {
                DisplayBuildingInfo(buildingInfo);
            }
            
            // 아이템
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
            // Fold out. 각 아이템은 박스처리
            foreach (var item in account.Items)
            {
                DisplayItem(item);
            }
        }
        
        // 스크롤뷰 끝
        EditorGUILayout.EndScrollView();
    }
    
    private void DisplayBuilding(BuildingDTO building)
    {
        GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
        
        EditorGUILayout.LabelField($"ID: {building.Id}");
        EditorGUILayout.LabelField($"BuildingType: {building.BuildingType}");
        EditorGUILayout.LabelField($"ActivatedAt: {building.ActivatedAt}");
        DisplayArrayHorizontal("Position", building.Position);
        DisplayArrayHorizontal("ProduceStatus", building.ProduceStatus);
        DisplayArrayHorizontal("Recipes", building.Recipes);
        DisplayArrayHorizontal("ProduceStartAt", building.ProduceStartAt);
        DisplayArrayHorizontal("ProduceEndAt", building.ProduceEndAt);
        
        GUILayout.EndVertical();
    }
    
    // Display items with index
    private void DisplayArrayHorizontal<T>(string name, T[] array)
    {
        EditorGUILayout.LabelField(name);
        GUILayout.BeginHorizontal();
        
        for (int i = 0; i < array.Length; i++)
        {
            EditorGUILayout.LabelField($"[{i}]: {array[i]}");
        }
        
        GUILayout.EndHorizontal();
    }
    
    private void DisplayBuildingInfo(BuildingInfoDTO buildingInfo)
    {
        GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
        
        EditorGUILayout.LabelField($"BuildingType: {buildingInfo.BuildingType}");
        EditorGUILayout.LabelField($"OwnCount: {buildingInfo.OwnCount}");
        EditorGUILayout.LabelField($"MaxCount: {buildingInfo.MaxCount}");
        EditorGUILayout.LabelField($"InstallCount: {buildingInfo.InstallCount}");
        EditorGUILayout.LabelField($"ProduceSlotCount: {buildingInfo.ProduceSlotCount}");
        
        GUILayout.EndVertical();
    }
    
    private void DisplayItem(ItemDTO item)
    {
        GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
        
        EditorGUILayout.LabelField($"ItemType: {item.ItemType}");
        EditorGUILayout.LabelField($"Count: {item.Count}");
        
        GUILayout.EndVertical();
    }
}
