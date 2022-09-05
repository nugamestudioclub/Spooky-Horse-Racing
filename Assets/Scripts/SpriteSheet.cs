using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(
    fileName = nameof(SpriteSheet),
    menuName = nameof(SpriteSheet))
]
public class SpriteSheet : ScriptableObject
{
    [SerializeField]
    private Sprite[] sprites;

    public Sprite this[int index] => sprites[index];

    public int Count => sprites.Length;
}
