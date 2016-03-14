using UnityEngine;

using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageEffectPrefab;
    public int hp = 200;
    public float speed = 2;
    public int attackRate = 2;//攻击速率 表示多少秒攻击一次
    public float attackDistance = 2;
    public float damage = 20;//这个是攻击力
    public float downSpeed = 1f;
    private int hpTotal = 0;
    private float downDistance = 0;
    private float distance = 0;
    private float attackTimer = 0;
    private Transform bloodPoint;
    private CharacterController cc;
    private GameObject hpBarGameObject;
    private UISlider hpBarSlider;
    private GameObject hudTextGameObject;
    private HUDText hudText;


    void Start() {
        TranscriptManager._instance.enemyList.Add(this.gameObject);
        hpTotal = hp;
        bloodPoint = transform.Find("BloodPoint");
        cc = this.GetComponent<CharacterController>();
        InvokeRepeating("CalcDistance", 0,0.1f);
        Transform hpBarPoint = transform.Find("HpBarPoint");
        hpBarGameObject =  HpBarManager._instance.GetHpBar(hpBarPoint.gameObject);
        hpBarSlider = hpBarGameObject.transform.Find("Bg").GetComponent<UISlider>();

        hudTextGameObject = HpBarManager._instance.GetHudText(hpBarPoint.gameObject);
        hudText = hudTextGameObject.GetComponent<HUDText>();
    }

    void Update() {
        if (hp <= 0) {
            //移到地下
            downDistance += downSpeed * Time.deltaTime;
            transform.Translate(-transform.up * downSpeed*Time.deltaTime );
            if (downDistance > 4) {
                Destroy(this.gameObject);
            }
            return;
        }
        if (distance < attackDistance) {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackRate) {
                Transform player = TranscriptManager._instance.player.transform;
                Vector3 targetPos = player.position;
                targetPos.y = transform.position.y;
                transform.LookAt(targetPos);
                animation.Play("attack01");
                attackTimer = 0;
            }
            if (!animation.IsPlaying("attack01")) {
                animation.CrossFade("idle");
            }
        } else {
            animation.Play("walk");
            Move();
        }
    }

    void Move() {
        Transform player = TranscriptManager._instance.player.transform;
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        cc.SimpleMove(transform.forward * speed);
    }
    void CalcDistance() {
        Transform player = TranscriptManager._instance.player.transform;
        distance = Vector3.Distance(player.position, transform.position);
    }
	
    //收到攻击调用这个方法
    // 0,收到多少伤害
    // 1,后退的距离
    // 2,浮空的高度
    void TakeDamage(string args) {
        if (hp <= 0) return;
        Combo._instance.ComboPlus();
        string[] proArray = args.Split(',');
        //减去伤害值
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        hpBarSlider.value = (float)hp / hpTotal;
        hudText.Add("-" + damage, Color.red, 0.3f);
//        受到攻击的动画
        animation.Play("takedamage");//浮空和后退
        float backDistance = float.Parse(proArray[1]);
        float jumpHeight = float.Parse(proArray[2]);
        iTween.MoveBy(this.gameObject,            transform.InverseTransformDirection(TranscriptManager._instance.player.transform.forward) * backDistance             + Vector3.up * jumpHeight,            0.3f);//出血特效实例化
        GameObject.Instantiate(damageEffectPrefab, bloodPoint.transform.position , Quaternion.identity);
        if (hp <= 0) {
            Dead();
        }
    }
    void Attack()
    {
        Transform player = TranscriptManager._instance.player.transform;
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", damage,SendMessageOptions.DontRequireReceiver);
        }
    }
    //当死亡的时候调用这个方法
    void Dead()
    {
        TranscriptManager._instance.enemyList.Remove(this.gameObject);
        this.GetComponent<CharacterController>().enabled = false;
        Destroy(hpBarGameObject);
        Destroy(hudTextGameObject);
        //第一种死亡方式是播放死亡动画
        //第二种死亡方式是使用破碎效果
        int random = Random.Range(0, 10);
        if (random <= 7)
        {
            animation.Play("die");
        }
        else
        {
            this.GetComponentInChildren<MeshExploder>().Explode();
            this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    }
}
