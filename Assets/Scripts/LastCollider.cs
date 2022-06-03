using UnityEngine;

public class LastCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Application.LoadLevel("TestLevel");
        GUI.Box(new Rect(0,0,Screen.width/2,Screen.height/2),"Выхода нет");
    }
}
