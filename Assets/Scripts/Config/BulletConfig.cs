using UnityEngine;

public class BulletConfig : MonoBehaviour {
    public enum TypeChoice {
        Standard,
        Chained,
        Piercing
    };
    public TypeChoice Type;
    public float Damage = 1f;
    public float Speed = 1f;
}
