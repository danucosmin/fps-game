using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour
{

    public Transform tr_Player;
    public float f_RotSpeed = 3.0f, f_MoveSpeed = 3.0f;
    public GameObject head;
    public float head_RotSpeed = 6.0f;
    // Use this for initialization
    void Start()
    {

        tr_Player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        /*  head Looking  at Player*/
        head.transform.rotation = Quaternion.Slerp(head.transform.rotation
                                              , Quaternion.LookRotation(tr_Player.position - head.transform.position)
                                              , head_RotSpeed * Time.deltaTime);

        /* Move at Player*/
        transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;

        // zombie looking at player 
        transform.rotation = Quaternion.Slerp(transform.rotation
                                             , Quaternion.LookRotation(tr_Player.position - transform.position)
                                             , f_RotSpeed * Time.deltaTime);
    }
}