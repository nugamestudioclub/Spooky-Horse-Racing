using System;
using UnityEngine;

public class PlayerProfile
{
    public string Name { get; private set; }
    public SpriteSheet Knight { get; private set; }
    public SpriteSheet Horse { get; private set; }

    public static PlayerProfile Idle = new PlayerProfile("", null, null);

    public PlayerProfile(string name, SpriteSheet knight, SpriteSheet horse)
    {
        Name = name;
        Knight = knight;
        Horse = horse;
    }
}