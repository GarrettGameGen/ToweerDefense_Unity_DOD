using UnityEngine;

public class EnemyConfig : MonoBehaviour {
    public enum TypeChoice {
        Basic,
        Speedy,
        Tank,

    };
    public TypeChoice Type;
    public float Health = 5;
    public float Armor = 0;
}
