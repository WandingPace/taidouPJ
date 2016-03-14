using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    void Awake()
    {
        Transform posTransform = GameObject.Find("Player-pos").transform;
        string playerPrefabName = "Player-girl";
        if (PhotonEngine.Instance.role.IsMan)
        {
            playerPrefabName = "Player-boy";
        }
        GameObject playerGo = GameObject.Instantiate(Resources.Load("Player/" + playerPrefabName)) as GameObject;
        playerGo.transform.position = posTransform.position;
    }


    public static int GetRequireExpByLevel(int level) {
        //等差数列
        return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2);
    }
}
