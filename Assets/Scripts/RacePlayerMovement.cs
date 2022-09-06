using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RacePlayerMovement : MonoBehaviour
{
    public int ControllerId { get; set; }
    public abstract bool ControlEnabled { get; set; }
    public Vector2 Orientation { get; protected set; }

    public Vector2 Velocity { get; protected set; }

    public bool IsGrounded { get; protected set; }
}
