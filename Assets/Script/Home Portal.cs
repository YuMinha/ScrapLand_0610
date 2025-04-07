using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePortal : MonoBehaviour
{
    public Transform mapPos;
    public GameObject player;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {

            Debug.Log("ĳ���� �̵�");
            CharacterController controller = player.GetComponent<CharacterController>();

            Vector3 pos = mapPos.position;

            controller.enabled = false;
            player.transform.position = pos;
            controller.enabled = true;
            Debug.Log("ĳ���� �̵� �Ϸ�");
        }
    }
}
