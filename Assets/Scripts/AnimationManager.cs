using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	[SerializeField]
	private SpriteSheet spriteSheet;
	public SpriteSheet SpriteSheet {
		get => spriteSheet;
		set => spriteSheet = value;
	}

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private int index;

	public int Index {
		get => index;
		set {
			index = value;
			spriteRenderer.sprite = spriteSheet[Index];
		}
	}
}
