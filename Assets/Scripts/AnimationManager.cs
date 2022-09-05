using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private SpriteSheet spriteSheet;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    private int index;

    public int Index
    {
        get => index;
        set
        {
            index = value;
            spriteRenderer.sprite = spriteSheet[Index];
        }
    }

    public bool ChangeSpritePack(SpriteSheet spriteSheet)
    {
        if (this.spriteSheet.Count == spriteSheet.Count)
        {
            this.spriteSheet = spriteSheet;
            return true;
        }
        return false;
    }
}
