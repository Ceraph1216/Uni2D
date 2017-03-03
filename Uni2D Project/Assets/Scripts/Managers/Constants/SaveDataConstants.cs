using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveDataConstants : MonoBehaviour
{
	[System.NonSerialized] public string SAVE_DATA_ENCRYPTION_PASSWORD = "th1s1ss@v3d@t@";

	// Options
	[System.NonSerialized] public string MUSIC_ENABLED_KEY = "musicEnabled";
	[System.NonSerialized] public string SOUNDS_ENABLED_KEY = "soundsEnabled";
	[System.NonSerialized] public string LANGUAGE_KEY = "language";

	// Records
	[System.NonSerialized] public string HIGH_SCORE_KEY = "HighScoreForLevel{0}";
	[System.NonSerialized] public string TOTAL_GOLD_KEY = "TotalGold";
	[System.NonSerialized] public string LIFETIME_GOLD_KEY = "LifetimeGold";

	// Upgrades
	[System.NonSerialized] public string ATTACK_UPGRADE_KEY = "AttackUpgradeLevel";
	[System.NonSerialized] public string HEALTH_UPGRADE_KEY = "HealthUpgradeLevel";
	[System.NonSerialized] public string DOUBLE_COINS_UPGRADE_KEY = "DoubleCoinsUpgrade";
}