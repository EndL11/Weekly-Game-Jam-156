using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoodleType
{
    public NoodleTypes.types type;
    public Sprite sprite;
}

public class NoodleDB : MonoBehaviour
{
    public NoodleType[] noodles;
}
