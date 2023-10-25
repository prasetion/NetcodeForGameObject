using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class AssetManager : MonoBehaviour
{

    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button createButton;

    [SerializeField] GameObject creationPrefab;

    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            createButton.gameObject.SetActive(true);
            hostButton.gameObject.SetActive(false);
            clientButton.gameObject.SetActive(false);
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            hostButton.gameObject.SetActive(false);
            clientButton.gameObject.SetActive(false);
        });

        createButton.onClick.AddListener(CreateObject);
        NetworkManager.Singleton.AddNetworkPrefab(creationPrefab);
    }

    // Update is called once per frame
    void CreateObject()
    {
        GameObject creation = Instantiate(creationPrefab, Vector3.up, Quaternion.identity);
        creation.GetComponent<NetworkObject>().Spawn();
    }
}
