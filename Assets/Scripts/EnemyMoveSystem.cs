using System.Collections;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyMoveSystem : ComponentSystem {
    //Alignment, Align Forward Directions
    //Cohesion, Move towards Center of Nearby Dudes
    //Separation, Give Space to others
    //Enemy
    //List<Transform> eSpawner = new List<Transform>();
    //List<SpawnConfig> spawnConfig = new List<SpawnConfig>();
    List<Vector3> pos = new List<Vector3>();
    List<Vector3> forw = new List<Vector3>();

    struct Enemy
    {
        //THESE VARIBLES ARE COMPONENTS ON THE ENEMY
#pragma warning disable 649
        public Transform transform;
        public EnemyConfig config;
        public MeshRenderer renderer;
#pragma warning restore 649

        //Methods in this Struct Are Only here to Set Data

    }
    struct Cell
    {
        public NativeArray<int> Index;
        public NativeArray<Vector3> Position;
        public NativeArray<Vector3> Heading;
        public NativeArray<int> Count;
    }
    protected override void OnStartRunning()
    {
        Debug.Log(GetEntities<Enemy>().Length);
    }
    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        float forwCount = 0;
        float posCount = 0;
        float turnSpeed = 5f;

        float cellRadius = 5f;
        float AlignWeight = 1f;
        float SeprationWeight = 1f;
        float TargetWeight = 0.5f;
        float RandHeadingWeight = 1f;
        
        //Generate Curent Positions and Forward Directions

        foreach (var e in GetEntities<Enemy>())
        {
            pos.Add(e.transform.position);
            forw.Add(e.transform.forward);
        }
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
            for (int i = 0; i < posCount; i++) {
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
            Heading = Vector3.Normalize(AlginmentResult + SeperationResult + RandHeading + TargetHeading);
            if (position.y < 0f) {
                Heading = Vector3.up;
            } else if (position.y > 20f)
            {
                Heading = Vector3.down;
            } else if (position.z > 50f)
            {
                Heading = Vector3.back;
            }
            else if (position.z < -50f)
            {
                Heading = Vector3.forward;
            }
            else if (position.x < -80f)
            {
                Heading = Vector3.right;
            }
            else if (position.x > 80f)
            {
                Heading = Vector3.left;
            }
            e.config.Heading = Heading;
            //Move and Rotate
            Quaternion rot = Quaternion.LookRotation(Vector3.Normalize(e.transform.forward + Heading));
            e.transform.rotation = Quaternion.Lerp(e.transform.rotation, rot, turnSpeed* dt);
            e.transform.position = position + e.transform.forward * dt * e.config.Speed;
            e.renderer.material.color = new Color(count*0.2f,0.5f,0.5f);
        }
        forw.Clear();
        pos.Clear();
    }
    //BulletManager bulletManager;
    //public GameObject _Enemy;
    // Use this for initialization
    /*
    void Start() {
        bulletManager = gameObject.GetComponent<BulletManager>();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemySpawner").Length; i++)
        {
            GameObject current = GameObject.FindGameObjectsWithTag("EnemySpawner")[i];
            eSpawner.Add(current.transform);
            spawnConfig.Add(current.gameObject.GetComponent<SpawnConfig>());
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Enemy Spawner
        if (eSpawner != null) {
            for (int i = 0; i < eSpawner.Count; i++)
            {
                if (spawnConfig[i].total > spawnConfig[i].deployed)
                { 
                    spawnConfig[i].timer += Time.deltaTime;
                    if (spawnConfig[i].timer >= spawnConfig[i].spawnRate)
                    {
                        spawnConfig[i].deployed++;
                        spawnConfig[i].timer = 0;
                        GameObject enemy = Instantiate(_Enemy);
                        enemy.transform.position = eSpawner[i].position;
                        eTransform.Add(enemy.transform);
                        eNav.Add(enemy.GetComponent<NavMeshAgent>());
                        config.Add(enemy.GetComponent<EnemyConfig>());
                    }
                }
            }
        }
        //Enemies
        if (config != null) {
            for (int i = 0; i < eTransform.Count; i++) {
                Transform trans = eTransform[i];
                NavMeshAgent agent = eNav[i];
                if (trans != null)
                {
                    //Damage
                    config[i].Health -= bulletManager.EnemyCollision(trans);
                    if (config[i].Health <= 0) {
                        removeEnemy(i);
                    }
                    //Move
                    agent.speed = config[i].Speed;
                    agent.destination = Vector3.zero;
                }
            }
        }
	}
    void removeEnemy(int index) {
        eTransform.RemoveAt(index);
        eNav.RemoveAt(index);
        config.RemoveAt(index);

        Destroy(eTransform[index].gameObject);
    }
    */
}
