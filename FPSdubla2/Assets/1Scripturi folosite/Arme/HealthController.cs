using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{

    // Use this for initialization


    [SerializeField]private float health = 100f;
   // [SerializeField] private Text textHealth;


    void Start()
    {

       // UpdateTextHealth();

    }




   public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

       // UpdateTextHealth();
    }


  //  public void UpdateTextHealth()
  //  {

    //    textHealth.text =  "HP:"+health.ToString();

  //  }
}