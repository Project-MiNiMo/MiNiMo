﻿using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

// Player Entity
public class Account
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string Nickname { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public DateTime LastLogin { get; set; }
    
    // 재화
    public Currency Currency { get; set; } = new Currency();
    
    // 건물
    public List<Building> Buildings { get; set; }
    
    // 건물 정보(소유, 해금)
    public List<BuildingInfo> BuildingInfos { get; set; }

    // 아이템
    public List<Item> Items { get; set; }
    
    // 유성
    public List<Meteor> Meteors { get; set; }
    public DateTime LastMeteorCreatedAt { get; set; }
    // 퀘스트
    public List<Quest> Quests { get; set; }
    
    // 별나무
    public DateTime LastStarTreeCreatedAt { get; set; }
    public DateTime LastWishedAt { get; set; }
    public int StarTreeLevel { get; set; }
    
    // 미니모
    // 친구
}