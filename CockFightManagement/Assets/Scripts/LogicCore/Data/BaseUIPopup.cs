using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPopup : MonoBehaviour
{
    public virtual void OnShow()
    {

    }
    public virtual void OnClickClose()
    {
        GameManager.Instance.HideUI(this);
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
