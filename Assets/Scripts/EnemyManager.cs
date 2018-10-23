using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    //Enemy
    List<Transform> eSpawner = new List<Transform>();
    List<SpawnConfig> spawnConfig = new List<SpawnConfig>();

    List<Transform> eTransform = new List<Transform>();
    List<NavMeshAgent> eNav = new List<NavMeshAgent>();
    List<EnemyConfig> config = new List<EnemyConfig>();

    BulletManager bulletManager;
    public GameObject _Enemy;
    // Use this for initialization
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

}
