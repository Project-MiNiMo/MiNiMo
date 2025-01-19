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

    private void Update()
    {
        Repaint();
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
            foreach (var building in account.Buildings)
            {
                DisplayBuilding(building);
            }
            
            // 빌딩 정보
            EditorGUILayout.LabelField("Building Info", EditorStyles.boldLabel);
            foreach (var buildingInfo in account.BuildingInfos)
            {
                DisplayBuildingInfo(buildingInfo);
            }
            
            // 아이템
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
            foreach (var item in account.Items)
            {
                DisplayItem(item);
            }
            
            // 유성
            EditorGUILayout.LabelField("Meteors", EditorStyles.boldLabel);
            foreach (var meteor in account.Meteors)
            {
                DisplayMeteor(meteor);
            }
            
            // 별나무
            EditorGUILayout.LabelField("Star Tree", EditorStyles.boldLabel);
            GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
            EditorGUILayout.LabelField($"Last Star Tree Created At: {account.LastStarTreeCreatedAt}");
            EditorGUILayout.LabelField($"Last Wished At: {account.LastWishedAt}");
            EditorGUILayout.LabelField($"Star Tree Level: {account.StarTreeLevel}");
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
    
    private void DisplayMeteor(MeteorDTO meteor)
    {
        GUILayout.BeginVertical("window", new GUILayoutOption[] { GUILayout.Height(60) });
        
        EditorGUILayout.LabelField($"ID: {meteor.Id}");
        EditorGUILayout.LabelField($"Type: {meteor.MeteorType}");
        EditorGUILayout.LabelField($"ValueIndex: {meteor.ValueIndex}");
        EditorGUILayout.LabelField($"ValueCount: {meteor.ValueCount}");
        
        GUILayout.EndVertical();
    }
}
