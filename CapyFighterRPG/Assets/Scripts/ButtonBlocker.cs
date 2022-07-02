using UnityEngine;
using System;

public class ButtonBlocker : MonoBehaviour
{
    public Predicate<int> Predicate { get; private set; }
}
