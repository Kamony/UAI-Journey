using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerScript : MonoBehaviour {

    // metoda volana v pripade colize s jinym objektem
    void OnTriggerEnter2D( Collider2D collidedObject)
    {
        // zavola metodu hitDeathTrigger
        // https://docs.unity3d.com/ScriptReference/GameObject.SendMessage.html - SendMessage
        collidedObject.SendMessage("hitDeathTrigger", SendMessageOptions.DontRequireReceiver);
    }
}
