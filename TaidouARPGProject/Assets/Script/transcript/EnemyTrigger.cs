using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour {

    public GameObject[] enemyPrefabs;
    public Transform[] spawnPosArray;
    public float time = 0;//表示多少秒之后开始生成
    public float repeateRate = 0;
    private bool isSpawned = false;

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" && isSpawned==false ) {
            isSpawned = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy() {
        yield return new WaitForSeconds(time);

        foreach (GameObject go in enemyPrefabs) {
            foreach (Transform t in spawnPosArray) {
                GameObject.Instantiate(go, t.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(repeateRate);
        }
    }

}
