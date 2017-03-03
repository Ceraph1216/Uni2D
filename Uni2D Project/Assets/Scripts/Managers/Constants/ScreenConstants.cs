using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenConstants : MonoBehaviour
{
	[System.NonSerialized] public string LEVEL_BASE_PATH = "Prefabs/Levels/";
	[System.NonSerialized] public string LEVEL_BASE_NAME = "Level_{0}_Tilemap";

	[System.NonSerialized] public string TITLE_SCREEN_NAME = "TitleScene";
	[System.NonSerialized] public string LEVEL_SELECT_SCREEN_NAME = "LevelSelectScreen";
	[System.NonSerialized] public string GAME_SCREEN_NAME = "GameScene";
	[System.NonSerialized] public string LEVEL_SCREEN_NAME_BASE = "Level";

	[System.NonSerialized] public string SHOP_SCREEN_NAME = "ShopScreen";
}