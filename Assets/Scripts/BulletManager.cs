using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    //Bullet
    List<Transform> bTransform;
    List<BulletConfig> config;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //Bullets
        for (int i = 0; i < bTransform.Count; i++)
        {
            bTransform[i].Translate(Vector3.forward * Time.deltaTime);
        }
    }
    public float EnemyCollision(Transform eTransform)
    {
        float totalDamage = 0;
        for (int i = 0; i < bTransform.Count; i++)
        {
            if (eTransform != null)
            {
                float dis = Vector3.Distance(eTransform.position, bTransform[i].position);
                if (dis <= 0.5f)
                {
                    totalDamage += config[i].Damage;
                    removeBullet(i);
                }
            }
        }
        return totalDamage;
    }
    public void addBullet(GameObject bullet)
    {
        bTransform.Add(bullet.transform);
        config.Add(bullet.GetComponent<BulletConfig>());

    }
    void removeBullet(int index)
    {
        bTransform.RemoveAt(index);
        config.RemoveAt(index);

        Destroy(bTransform[index].gameObject);
    }
}
