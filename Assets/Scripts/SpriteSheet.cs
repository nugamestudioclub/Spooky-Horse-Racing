using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(
    fileName = nameof(SpriteSheet),
    menuName = nameof(SpriteSheet))
]
public class SpriteSheet : ScriptableObject, IReadOnlyCollection<Sprite>
{
    [SerializeField]
    private Sprite[] sprites;

    public Sprite this[int index] => sprites[index % Count];

    public int Count => sprites.Length;

	public IEnumerator<Sprite> GetEnumerator() {
		return (sprites as IEnumerable<Sprite>).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return sprites.GetEnumerator();
	}
}