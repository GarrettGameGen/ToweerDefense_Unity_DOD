using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    //Enemy
    List<Transform> eTransform;
    List<NavMeshAgent> eNav;
    List<EnemyConfig> config;

    BulletManager bulletManager;

    // Use this for initialization
    void Start() {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++) {
            GameObject current = GameObject.FindGameObjectsWithTag("Enemy")[i];
            if(current != null)
                addEnemy(current);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Enemies
        for(int i = 0; i < eTransform.Count; i++) {
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
                agent.destination = Vector3.zero;
            }
        }
	}
    void addEnemy(GameObject enemy) {
        Debug.Log(enemy);
        eTransform.Add(enemy.transform);
        eNav.Add(enemy.GetComponent<NavMeshAgent>());
        config.Add(enemy.GetComponent<EnemyConfig>());
    }
    void removeEnemy(int index) {
        eTransform.RemoveAt(index);
        eNav.RemoveAt(index);
        config.RemoveAt(index);

        Destroy(eTransform[index].gameObject);
    }

}
