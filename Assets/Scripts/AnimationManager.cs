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
	private SpriteRenderer[] spriteRenderers;

	private int index;

	public int Index {
		get => index;
		set {
			index = value;
			spriteRenderers[0].sprite = spriteSheet[Index];
		}
	}

	public void SetPieces(SpriteSheet pieces)
    {
		for (int i = 0; i < pieces.Count;i++)
        {
			if (spriteRenderers[i] != null)
            {
				spriteRenderers[i].sprite = pieces[i];
			}
			
        }
    }

	public void FlipPiecesX(bool flip)
    {
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			if (spriteRenderers[i] != null)
			{
				spriteRenderers[i].flipX = flip;
			}

		}
	}
}
