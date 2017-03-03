using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayConstants : MonoBehaviour
{
	public int NUM_STARTING_LIVES;

	public int NUM_LANES;
	public float SPACE_BETWEEN_LANES;

	public float MOVEMENT_SPEED;

	public int COIN_PICKUP_SCORE;

	public float TIME_TO_BLINK_ON_HIT;

	public LayerMask INTERACTABLE_LAYER;
}