using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public float x,y,z;
    string filepath; 

    public CharacterController cc;
    public PauseMenu pm;
    private PlayerController player;
    public GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManager = gameManager.GetComponent<GameManager>();
        filepath = Path.Combine(Application.dataPath, "playerData.dat");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        PlayerData data = new PlayerData();

        data.setX(transform.position.x);
        data.setY(transform.position.y);
        data.setZ(transform.position.z);
        data.SetArmor(player.armaturaAcquisita);
        data.SetSpada(player.spadaAcquisita);
        data.SetQuestMeleT(gameManager.questMeleTerminata);

        Stream stream = new FileStream(filepath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);
        stream.Close();
        
    }

    public void Load(){

        if (File.Exists(filepath))
        {
            Stream stream = new FileStream(filepath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            PlayerData data = (PlayerData)bf.Deserialize(stream);
            stream.Close();

            cc.enabled = false;
            
            transform.position = new Vector3(data.GetX(), data.GetY(), data.GetZ());
            gameManager.questMeleTerminata = data.GetQuestMeleT();
            player.armaturaAcquisita = data.GetArmor();
            player.spadaAcquisita = data.GetSpada();
            
            cc.enabled = true;

            pm.Resume();
        }
    }
}
