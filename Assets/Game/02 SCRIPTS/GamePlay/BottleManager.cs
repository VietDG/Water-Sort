using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : SingletonMonoBehaviour<BottleManager>
{
    [SerializeField] GameObject _waterPrefab;
    [SerializeField] float _spaceHorizontal, _spaceVertical, _tubeHorizontalMax;
    public List<BottleController> bottleList = new List<BottleController>();
    public BottleController _holdingBottle;


    public void OnClick(BottleController newBottle)
    {
        if (_holdingBottle == null)
        {
            if (newBottle.isEmpty()) return;
            _holdingBottle = newBottle;
            newBottle.StartMove(newBottle, true);
        }
        else
        {
            if (_holdingBottle.Equals(newBottle))
            {
                Debug.LogError("1");
                newBottle.StartMove(newBottle, false, 0);
                _holdingBottle = null;
            }
            else
            {
                Debug.LogError("2");
                if (!newBottle.CanSortBall(_holdingBottle))
                {
                    _holdingBottle.StartMove(_holdingBottle, false, 0);
                    newBottle.StartMove(newBottle, true);
                    _holdingBottle = newBottle;
                }
                else
                {
                    _holdingBottle.StartColorTransfer(newBottle);
                    _holdingBottle = null;
                }
            }
        }
    }
}
