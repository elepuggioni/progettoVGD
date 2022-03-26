using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public float x,y,z; // Posizione del Player
    string filepath;  // path del file

    // Riferimenti
    public PauseMenu pm;
    public CharacterController cc;
    private PlayerController player;
    public GameManager gameManager;
    private GameObject viceCapo;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManager = gameManager.GetComponent<GameManager>();
        viceCapo = GameObject.FindGameObjectWithTag("ViceCapo");

        filepath = Path.Combine(Application.dataPath, "playerData.dat"); // Creo il path
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        PlayerData data = new PlayerData(); // Creo dei nuovi dati 

        // Aggiorno i vecchi dati con i nuovi 
        data.setX(transform.position.x);
        data.setY(transform.position.y);
        data.setZ(transform.position.z);
        data.SetArmor(player.armaturaAcquisita);
        data.SetSpada(player.spadaAcquisita);
        data.SetQuestMeleT(gameManager.questMeleTerminata);
        data.SetViceCapoDead(viceCapo.GetComponent<ViceCapoController>().isDead);

        Stream stream = new FileStream(filepath, FileMode.Create); // Creo il file
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data); // Inserico i dati nel file in formato binario
        stream.Close(); //chiudo il file
        
    }

    public void Load(){

        // Se il file esiste
        if (File.Exists(filepath))
        {
            Stream stream = new FileStream(filepath, FileMode.Open); // Apro il file
            BinaryFormatter bf = new BinaryFormatter();
            PlayerData data = (PlayerData)bf.Deserialize(stream); // Prendo i dati dal file
            stream.Close();

            cc.enabled = false; // Disabilito il character controller
            
            // Inserisco i dati salvati nella partita corrente
            transform.position = new Vector3(data.GetX(), data.GetY(), data.GetZ());
            gameManager.questMeleTerminata = data.GetQuestMeleT();
            player.armaturaAcquisita = data.GetArmor();
            player.spadaAcquisita = data.GetSpada();
            if (data.GetViceCapoDead())
                viceCapo.SetActive(false);
            else
                viceCapo.SetActive(true);


                cc.enabled = true;

            pm.Resume(); // Riprendo il gioco
        }
    }
}
