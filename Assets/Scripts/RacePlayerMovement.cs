using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RacePlayerMovement : MonoBehaviour
{
    public int ControllerId { get; set; }
    public bool ControlEnabled { get; set; }
    public Vector2 Orientation { get; protected set; }

    public Vector2 Velocity { get; protected set; }

    public bool IsGrounded { get; protected set; }

    public IList<SerializableTransformData> Data { get; protected set; }

    public abstract void Play();

    public abstract void Stop();

    public abstract void Freeze();

    public abstract void Freeze(float duration);

    public abstract void UnFreeze();
}
