using UnityEngine;
using System.Collections;

public class TranscriptMapDialog : MonoBehaviour {

    private UILabel desLabel;
    private UILabel energyTagLabel;
    private UILabel energyLabel;
    private UIButton enterButton;
    private UIButton closeButton;
    private TweenScale tween;

    void Start() {
        desLabel = transform.Find("Sprite/DesLabel").GetComponent<UILabel>();
        energyTagLabel = transform.Find("Sprite/EnergyTagLabel").GetComponent<UILabel>();
        energyLabel = transform.Find("Sprite/EnergyLabel").GetComponent<UILabel>();
        enterButton = transform.Find("BtnEnter").GetComponent<UIButton>();
        closeButton = transform.Find("BtnClose").GetComponent<UIButton>();
        tween = this.GetComponent<TweenScale>();

        EventDelegate ed1 = new EventDelegate(this, "OnEnter");
        enterButton.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnClose");
        closeButton.onClick.Add(ed2);
    }

    public void ShowWarn() {
        energyLabel.enabled = false;
        energyTagLabel.enabled = false;
        enterButton.enabled = false;

        desLabel.text = "当前等级无法进入该地下城";
        tween.PlayForward();
    }
    public void ShowDialog(BtnTranscript transcript) {
        energyLabel.enabled = true;
        energyTagLabel.enabled = true;
        enterButton.enabled = true;

        desLabel.text = transcript.des;
        energyLabel.text = "3";
        tween.PlayForward();
    }

    void OnEnter() {
        transform.parent.SendMessage("OnEnter");
    }
    void OnClose() {
        tween.PlayReverse();
    }

}
