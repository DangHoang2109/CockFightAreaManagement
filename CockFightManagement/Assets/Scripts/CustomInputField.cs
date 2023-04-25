using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CustomInputField : TMP_InputField
{
    public System.Action<CustomInputField, string> onCustomSubmit;
    public System.Action<CustomInputField,string> onCustomSelected;
    public System.Action<CustomInputField, string> onCustomDeSelected;
    public System.Action<CustomInputField, string> onCustomValueChanged;
    public System.Action<CustomInputField, string> onCustomEndEdit;

    protected override void Start()
    {
        this.onSubmit.RemoveListener(OnBeingSubmit);
        this.onSubmit.AddListener(OnBeingSubmit);
    }

    public void OnBeingSubmit(string inp)
    {
        onCustomSubmit?.Invoke(this, this.text);
    }
    public void OnSelected(string inp)
    {
        onCustomSelected?.Invoke(this, this.text);
    }
    public void OnBeingValueChanged(string ipt)
    {
        onCustomValueChanged?.Invoke(this, ipt);
    }
    public void OnBeingDeselected (string ipt)
    {
        onCustomDeSelected?.Invoke(this, ipt);
    }
    public void OnBeingEndEdit(string ipt)
    {
        onCustomEndEdit?.Invoke(this, ipt);
    }
}
