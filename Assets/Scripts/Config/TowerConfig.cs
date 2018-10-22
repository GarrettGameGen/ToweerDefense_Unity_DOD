using UnityEngine;

public class TowerConfig : MonoBehaviour {
    public enum TypeChoice {
        Fire, //Continous Damage
        Earth, //Slow High Damage.
        Ice, //Stuns
        Wind, //Slows
        Electric, //Chained
        Nuclear, //Dot
    };
    public bool towerVpit = true;
    public TypeChoice Type;
    public float range; 
    [Tooltip("Rate of Fire per Second")]
    public float RoF;
    
}
