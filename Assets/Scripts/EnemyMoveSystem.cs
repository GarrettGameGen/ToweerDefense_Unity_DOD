using System.Collections;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics; 
using Unity.Transforms; //Position, Rotation, Scale Etc

namespace GameGen
{
    public class EnemyMoveSystem : JobComponentSystem
    {
        //Alignment, Align Forward Directions
        //Cohesion, Move towards Center of Nearby Dudes
        //Separation, Give Space to others
        //Enemy
        //List<Transform> eSpawner = new List<Transform>();
        //List<SpawnConfig> spawnConfig = new List<SpawnConfig>();
        private ComponentGroup m_BoidGroup;

        List<Vector3> pos = new List<Vector3>();
        List<Vector3> forw = new List<Vector3>();

        List<Cell> cells = new List<Cell>();
        List<Boid> boids = new List<Boid>();
        List<Enemy> enemies = new List<Enemy>();

        struct Enemy //Component Group
        {
            //THESE VARIBLES ARE COMPONENTS ON THE ENEMY
#pragma warning disable 649
            public Transform transform;
            public EnemyConfig config;
            public BoidConfig boidConfig;
            public MeshRenderer renderer;
#pragma warning restore 649
        }
        struct Cell //Temp Varible Storage Containter for Jobs 
        {
            public NativeArray<Vector3> Position;
            public NativeArray<Quaternion> Rotation;
            public NativeArray<Vector3> Heading;
        }
        /*
        struct Steer
        {
            public NativeArray<Vector3> positions;
            public NativeArray<Vector3> headings;
            public float dt;
            public void Execute(int index)
            {

                headings[index] = Vector3.Normalize(AlginmentResult + SeperationResult + RandHeading + TargetHeading);
            }
        }
        struct BoundingBox
        {
            public NativeArray<Vector3> positions;
            public NativeArray<Vector3> headings;
            public void Execute(int index)
            {
                var heading = headings[index];
                var position = positions[index];
                if (position.y < 0f)
                {
                    heading = Vector3.up;
                }
                else if (position.y > 20f)
                {
                    heading = Vector3.down;
                }
                else if (position.z > 50f)
                {
                    heading = Vector3.back;
                }
                else if (position.z < -50f)
                {
                    heading = Vector3.forward;
                }
                else if (position.x < -80f)
                {
                    heading = Vector3.right;
                }
                else if (position.x > 80f)
                {
                    heading = Vector3.left;
                }
                headings[index] = heading;
            }
        }
        */
        [BurstCompile]
        struct Move : IJobParallelFor
        {
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Rotation> rotations;
            //public NativeArray<Vector3> headings;
            public float dt; 

            public void Execute(int i)
            {
                Debug.Log("Hello?");
                float turnSpeed = 5.0f;
                float ForwSpeed = 10.0f;
                var position = positions[i].Value;
                var rotation = rotations[i].Value;
                var targetForward = math.forward(rotation);
                //Quaternion rot = Quaternion.LookRotation(Vector3.Normalize(targetForward + headings[i]));
                //rotations[i] = Quaternion.Lerp(rotations[i], rot, turnSpeed * dt);
                positions[i] = position + Position. targetForward * ForwSpeed * dt;
                //e.renderer.material.color = new Color(count * 0.2f, 0.5f, 0.5f);
            }
        }
        protected override void OnStartRunning()
        {
            Debug.Log(GetEntities<Enemy>().Length);
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityManager.GetAllUniqueSharedComponentData(boids);
            float deltime = Time.deltaTime;

            //Generate Curent Positions and Forward Directions
            int enemyCount = boids.Count;
            for (int typeIndex = 0; typeIndex < boids.Count; typeIndex++)
            {
                var boidConfig = boids[typeIndex];
                m_BoidGroup.SetFilter(boidConfig);
                //Enemy enemy = enemies[typeIndex];

                //Define Values
                var position = m_BoidGroup.GetComponentDataArray<Position>();
                var rotation = m_BoidGroup.GetComponentDataArray<Rotation>();
                //var heading = m_BoidGroup.GetComponentDataArray<Heading>();
                //new NativeArray<Vector3>(enemyCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                int entCount = GetEntities<Enemy>().Length;
                /*
                var nextCell = new Cell
                {
                    Position = position,
                    Rotation = rotation,
                    Heading = heading,
                };
                if (typeIndex >= cells.Count)
                {
                    cells.Add(nextCell);
                }
                else
                {
                    cells[typeIndex].Position.Dispose();
                    cells[typeIndex].Heading.Dispose();
                }
                cells[typeIndex] = nextCell;
                */
                //Jobs
                /*Steer SteerJob = new Steer
                {
                    positions = position,
                    headings = heading,
                    dt = Time.deltaTime,
                };
                var SteerSteerJob.Execute(typeIndex);
                */
                Move moveJob = new Move
                {
                    positions = position,
                    rotations = rotation,
                    //headings = heading,
                    dt = deltime,
                };
                var moveJobHandle = moveJob.Schedule(entCount, 64, inputDeps);
                inputDeps = moveJobHandle;
            }
            return inputDeps;
        }
    }
}
/*
posCount = pos.Count;
forwCount = forw.Count;
foreach (var e in GetEntities<Enemy>()) //Foreach loops are Read only for the Value e; BAH
{

    //find Heading
    Vector3 position = e.transform.position;
    Vector3 Heading = e.config.Heading;
    Vector3 AlginmentResult = Vector3.zero;
    Vector3 SeperationResult = Vector3.zero;
    Vector3 TargetHeading = Vector3.zero;

    Vector3 avgforw = Vector3.zero;
    Vector3 avgPos = Vector3.zero;

    int count = 0;
    for (int i = 0; i < posCount; i++)
    {
        float dis = Vector3.Distance(position, pos[i]);
        if (dis < cellRadius)
        {
            avgforw += forw[i];
            avgPos += pos[i];
            count++;
        }
    }
    Vector3 RandHeading = RandHeadingWeight * Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
    if (count > 1)
    {
        avgforw /= count;
        avgPos /= count;
        Vector3 offset = position - avgPos;
        AlginmentResult = AlignWeight * Vector3.Normalize(avgforw); //- offset
                                                                    //Debug.DrawRay(avgPos,Vector3.one/3,Color.green);
                                                                    //Debug.DrawRay(e.transform.position, Heading, Color.red);
        float sepration = -1f;
        //SeperationResult = SeprationWeight * Vector3.Normalize(offset * sepration);
    }
                
                
    e.config.Heading = Heading;
    //Move and Rotate
                
                
}
*/
