using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    bool toRight = true;
    public float moveSpeed = 1f;

    //public enum MOVESTATE
    //{
    //    MOVEX,
    //    MOVEZ,
    //    STOP
    //}

    public MOVESTATE currentState = MOVESTATE.STOP;

    void Start()
    {
        

    }

    void Update()
    {
        switch (currentState)
        {
            case MOVESTATE.MOVEX:
                MoveX();
                break;
            case MOVESTATE.MOVEZ:
                MoveZ();
                break;
            case MOVESTATE.STOP:
                break;
            default:
                break;
        }
    }

    void MoveX()
    {
        if (toRight)
        {
            this.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * moveSpeed);
            if (this.transform.position.x > 5)
                toRight = false;
        }
        else
        {
            this.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * moveSpeed);
            if (this.transform.position.x < -5)
                toRight = true;
        }  
    }

    void MoveZ()
    {
        if (toRight)
        {
            this.transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * moveSpeed);
            if (this.transform.position.z > 5)
                toRight = false;
        }
        else
        {
            this.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
            if (this.transform.position.z < -5)
                toRight = true;
        }
    }




}
