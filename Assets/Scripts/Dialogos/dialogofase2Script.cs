using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class dialogofase2Script : MonoBehaviour
{
    public NPCConversation dialogo; 
    void Start()
    {
        ConversationManager.Instance.StartConversation(dialogo);
    }

}
