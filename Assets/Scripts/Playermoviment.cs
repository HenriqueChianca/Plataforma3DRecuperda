using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Playermoviment : NetworkBehaviour
{

    //movimento
    Rigidbody fisica;

    bool estaNoChao;

    public Vector3 movimentoX;

    public float velocidade = 2;

    public float ForcaPulo = 10;

    SceneHandler sceneHandler;

    public Camera playerCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
            

        //Ativar Camera
        playerCamera.enabled = true;

        sceneHandler = GameObject.FindObjectOfType<SceneHandler>();
        
        movimentoX = new Vector3(0.0f, 0.1f, 0.0f);

        fisica = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //Despawnar players adicionais
        if (gameObject.GetComponent<NetworkObject>().IsPlayerObject != true && IsServer && gameObject.GetComponent<NetworkObject>().IsSpawned)
        {
            gameObject.GetComponent<NetworkObject>().Despawn(false);
        }

        if (!IsOwner)
            return;

        float movimentoX = Input.GetAxis("Horizontal");
        float movimentoZ = Input.GetAxis("Vertical");

        transform.position += new Vector3(movimentoX/10, 0, movimentoZ/10);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            fisica.velocity = Vector3.zero;
            fisica.AddForce(movimentoX, ForcaPulo, 0, ForceMode.Impulse);
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level1"))
        {
            sceneHandler.LoadLevel1();
        }
        else if (other.CompareTag("BackLobby"))
        {
            sceneHandler.LoadGame();
        }
    }
}
