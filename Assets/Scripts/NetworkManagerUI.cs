using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    private Button serverBtn;
    private Button hostBtn;
    private Button clientBtn;

    private void Awake() {
        serverBtn = gameObject.transform.GetChild(0).GetComponent<Button>();
        hostBtn = gameObject.transform.GetChild(1).GetComponent<Button>();
        clientBtn = gameObject.transform.GetChild(2).GetComponent<Button>();

        serverBtn.onClick.AddListener(() => { 
            NetworkManager.Singleton.StartServer();
        });

        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
