using SS.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] GameObject _handObj;
    private HandController _handController;
    public bool IsTutLevel1 { get; private set; }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void DisplayTutLevel1Step1(Vector2 target)
    {
        GameUIManager.Instance.DisplayGameUI(false);
        GameUIManager.Instance.DisplayTut(Const.KEY_TUT_LV_1, true);
        GameObject hand = SimplePool.Spawn(_handObj, Vector3.zero, Quaternion.identity);
        _handController = hand.GetComponent<HandController>();
        _handController.SetPosition(target);
        this.IsTutLevel1 = false;
    }

    public void DisplayTutLevel1Step2()
    {
        Vector2 target = GameManager.Instance.controller.bottleList[^1].originalPosition;
        _handController.Movement(target + new Vector2(1, -1));
        this.IsTutLevel1 = true;
    }

    public void DisplayTutLevel2Step1()
    {
        GameUIManager.Instance.DisplayTut(Const.KEY_TUT_LV_2, true);
        GameUIManager.Instance.DisplayGameUI(false);
        this.IsTutLevel1 = true;
    }

    public void DeactiveTut()
    {
        _handController?.Hide();
        GameUIManager.Instance.DisplayTut(null, false);
        this.IsTutLevel1 = false;
    }
}
