using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    public float viewAngle = 50;
    public float rotateSpeed = 1;
    public float attackDistance = 3;
    public float moveSpeed = 2;
    public float timeInterval = 1;
    public float[] attackArray;
    public GameObject bossBulletPrefab;

    private float timer = 0;
    private Transform player;
    private bool isAttacking = false;
    private GameObject attack01GameObject;
    private GameObject attack02GameObject;
    private Transform attack03Pos;
	// Use this for initialization
	void Start () {
        player = TranscriptManager._instance.player.transform;
        attack01GameObject = transform.Find("attack01").gameObject;
        attack02GameObject = transform.Find("attack02").gameObject;
        attack03Pos = transform.Find("attack03Pos");
	}
	
	// Update is called once per frame
	void Update () {
        if (isAttacking == true) return;
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;//保证夹角不受到y轴的影响
        float angle = Vector3.Angle(playerPos - transform.position, transform.forward); 

        if (angle < viewAngle / 2) {
            //在攻击视野之内
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < attackDistance) {
                //进行攻击
                if (isAttacking == false) {
                    animation.CrossFade("stand");
                    timer += Time.deltaTime;
                    if (timer > timeInterval) {
                        timer = 0;
                        Attack();
                    }
                }
            } else {
                //进行追击
                animation.CrossFade("walk");
                rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
            }
        } else {
            //在攻击视野之外 进行转向
            animation.CrossFade("walk");
            Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
	}
    private int attackIndex = 0;
    void Attack() {
        isAttacking = true;
        attackIndex++;
        if (attackIndex == 4) attackIndex = 1;
        animation.CrossFade("attack0" + attackIndex);
        //if (attackIndex == 1) {
        //    animation.CrossFade("attack01");
        //} else if (attackIndex == 2) {
        //    animation.CrossFade("attack02");
        //} else if (attackIndex == 3) {
        //    animation.CrossFade("attack03");
        //}
    }
    void BackToStand() {
        isAttacking = false;
    }

    void PlayAttack01Effect() {
        attack01GameObject.SetActive(true);

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance) {
            player.SendMessage("TakeDamage", attackArray[0], SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayAttack02Effect() {
        attack02GameObject.SetActive(true);

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance) {
            player.SendMessage("TakeDamage", attackArray[1], SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayAttack03Effect() {
        GameObject go = GameObject.Instantiate(bossBulletPrefab, attack03Pos.position, attack03Pos.rotation) as GameObject;
        BossBullet bb=  go.GetComponent<BossBullet>();
        bb.Damage = attackArray[2];
    }
}
