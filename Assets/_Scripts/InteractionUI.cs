using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionUI : Button
{
    public TMPro.TMP_Text interDescText;
    public string interactionDesc;
    IInteractable.Interaction InteractionDel;
    public int selID;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        InteractionDel();
    }

    public void Setup(TMPro.TMP_Text text, KeyValuePair<string, IInteractable.Interaction> inter, int id)
    {
        interDescText = text;
        interactionDesc = inter.Key;
        InteractionDel = inter.Value;
        selID = id;
    }
}
