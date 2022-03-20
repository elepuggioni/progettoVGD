using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData // Contenitore dei dati da salvare 
{
    private  float x, y, z; // posizione del player
    private bool armatura, spada, questMeleTerminata;

    public void setX(float x)
    {
        this.x = x; 
    }
    public void setY(float y)
    {
        this.y = y;
    }
    public void setZ(float z)
    {
        this.z = z;
    }

    public float GetX()
    {
        return this.x;
    }
    public float GetY()
    {
        return this.y;
    }
    public float GetZ()
    {
        return this.z;
    }

    public void SetArmor(bool armatura)
    {
        this.armatura = armatura;
    }
    public bool GetArmor()
    {
        return this.armatura;
    }

    public void SetSpada(bool spada)
    {
        this.spada = spada;
    }
    public bool GetSpada()
    {
        return this.spada;
    }

    public void SetQuestMeleT(bool mele)
    {
        this.questMeleTerminata = mele;
    }
    public bool GetQuestMeleT()
    {
        return this.questMeleTerminata;
    }

}
