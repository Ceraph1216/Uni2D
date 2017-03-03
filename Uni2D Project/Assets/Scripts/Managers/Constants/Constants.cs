using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constants : MonoBehaviour
{
	public const string BGM_SOUND = "bg_music.mp3";
	public const string BUTTON_SOUND = "button_click.mp3";
	public const string CORRECT_SOUND = "correct.mp3";
	public const string WRONG_SOUND = "wrong.mp3";
	public const string SWOOSH_SOUND = "swoosh.mp3";
	public const string SLIDE1_SOUND = "slide1.mp3";
	public const string SLIDE2_SOUND = "slide2.mp3";
	public const string SLIDE3_SOUND = "slide3.mp3";
	public const string SLIDE4_SOUND = "slide4.mp3";
	public const string JUMP_SOUND = "jump.mp3";
	public const string DEATH_SOUND = "death.mp3";
	public const string SLIDE_SOUND = "slide.mp3";
	public const string STUCK_SOUND = "splat.mp3";

	// UI
	public const float MAX_IMAGE_WIDTH = 170f;
	public const float MAX_IMAGE_HEIGHT = 85f;

	// Layer IDs
	public static int PLAYER_LAYER_ID = 8;
	public static int GROUND_LAYER_ID = 9;
	public static int ENEMY_LAYER = 10;

	// Game Dimensions
	public static int SCREEN_WIDTH = 1024;
	public static int SCREEN_HEIGHT = 768;

	// Player Movement
	public static float LANDING_STUN_TIME = 0.05f;
	public static float RUN_SPEED = 4f;
	public static float JUMP_FORCE = 9f;
	public static float SUPER_JUMP_FORCE = 13f;
	public static float SLIDE_DURATION = 0.4f;
	public static float SLIDE_COOLDOWN = 0.75f;
	public static float SLIDE_FORCE = 10f;
	public static float PLAYER_GRAVITY = 2f;
	public static float LAUNCH_TIMER = 0.6f;
	public static float SUPER_JUMP_CHARGE_TIME = 1f;
	public static float GRAVITY_FLIP_COOLDOWN = 0.25f;
	public static float END_LEVEL_LAUNCH_FORCE = 10f;

	static Constants mInstance;
	
	// The instance of the Constants class
	static public Constants instance
	{
		get
		{
			return mInstance;
		}
	}
	
	public ScreenConstants screens;
	public GameplayConstants gameplay;
	public SaveDataConstants saveData;

	void Awake()
	{
		mInstance = this;
	}
}