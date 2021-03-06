﻿using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : MonoBehaviour {
    public enum TypeChoice {
        Basic,
        Speedy,
        Tank,

    };
    public TypeChoice Type;
    public float MaxHealth = 5;
    [HideInInspector]
    public float Health = 5;
    public float Armor = 0;
    public float Speed = 3;
    public Color color;
}
