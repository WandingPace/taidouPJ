using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = this.GetComponent<Animator>();
    }

    public void OnAttackButtonClick ( bool isPress, PosType posType  )  {
        if (posType == PosType.Basic) {
            if (isPress)
                anim.SetTrigger("Attack");
        } else {
            anim.SetBool("Skill" + (int)posType, isPress);
        }
    }

}
