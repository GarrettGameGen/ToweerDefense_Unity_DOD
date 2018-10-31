using UnityEngine;
using System;
using Unity.Entities;

namespace GameGen { 
    [Serializable]
    public struct Boid : ISharedComponentData
    {
        public float cellRadius;
        public float separationWeight;
        public float alignmentWeight;
        public float targetWeight;
        public float obstacleAversionDistance;

        [HideInInspector]
        public Vector3 Heading;
    }
    public class BoidConfig : SharedComponentDataWrapper<Boid> { }
}
