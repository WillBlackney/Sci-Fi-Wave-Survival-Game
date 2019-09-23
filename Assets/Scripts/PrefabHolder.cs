using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    [Header("Buttons")]
    public GameObject AbilityButtonPrefab;
    public GameObject spellInfoPrefab;

    [Header("Defender Prefabs")]
    public GameObject spaceShipPrefab;
    public GameObject warriorPrefab;
    public GameObject magePrefab;
    public GameObject priestPrefab;
    public GameObject rangerPrefab;
    public GameObject roguePrefab;
    public GameObject shamanPrefab;

    [Header("World Object Prefabs")]
    public GameObject treePrefab;
    public GameObject rockWallPrefab;
    public GameObject rubblePrefab;

    [Header("Loot Related")]
    public GameObject GoldRewardButton;
    public GameObject ItemRewardButton;
    public GameObject ArtifactRewardButton;
    public GameObject ArtifactGO;
    public GameObject ItemCard;
    public GameObject TreasureChest;

    [Header("World/Level Related")]
    public GameObject LevelBG;

    [Header("Enemy Related")]
    public GameObject ZombiePrefab;

}
