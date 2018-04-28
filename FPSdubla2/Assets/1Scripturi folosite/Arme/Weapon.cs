


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    private Animator anim;
    private AudioSource _AudioSource;
    public Text ammoText;
    public Text fireSelectorText;
    public float range = 100f; // Maximum bullet range
    public int bulletsPerClip = 32; // Bullets per each magazine
    public int bulletsLeft = 250; // total bullets we have left
    public int currentBullets; // Current bullets in our clip
    public Camera FovChange;
    public float fovAmount;
    public float fovSmoothAmount;

    public enum ShootMode { Auto, Semi }
    public ShootMode shootingMode;

    public Transform shootPoint; // The point from where the bullet leaves the muzzle
    public GameObject hitParticles;
    public GameObject bulletImpact;


    public ParticleSystem MuzzleFlash; // Muzzle Flash
    public AudioClip shootSound; // Sound effect when shooting

    public float fireRate = 0.1f; // The delay between each shot
    public float damage = 20f;
    

    float fireTimer; // Timecounter for the delay

    //private bool isAiming;
    private bool isReloading;
    private bool shootInput;

    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float aodSpeed = 8;

    private bool semi = false;

    void OnEnable()
    {

        //Update cand este nevoie;
        UpdateAmmoText();
        UpdateFireSelectorText();

    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        currentBullets = bulletsPerClip;

        originalPosition = transform.localPosition;

        UpdateAmmoText();
        UpdateFireSelectorText();

    }

    // Update is called once per frame
    void Update()
    {



       
         //   switch (shootingMode)
         //   {
           //     case ShootMode.Auto:


                //    shootInput = Input.GetButton("Fire1");

               //     break;

              //  case ShootMode.Semi:


               //     shootInput = Input.GetButtonDown("Fire1");
              //      break;

           // }
        



          if (Input.GetKeyDown(KeyCode.B))
          {
               semi = !semi;


            }
          if (semi == true)
            {

                shootInput = Input.GetButtonDown("Fire1");

            }
           else if (semi == false )
             {

              shootInput = Input.GetButton("Fire1");
          }



        if (shootInput)
        {
            if (currentBullets > 0)
                Fire(); // Executes the fire fuction if we press / hold left mouse button
            else if (bulletsLeft > 0)
                DoReload(); // Automaticly reloads when having 0 bullets in clip
        }

        if (Input.GetKeyDown(KeyCode.R))
        {


            if (currentBullets < bulletsPerClip && bulletsLeft > 0)
                DoReload();
        }



        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime; // Add into timecounter
        }



        UpdateFireSelectorText();
        UpdateAmmoText();
        AimDownSights();
    }

    void FixedUpdate()
    {


        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName("Reload");


        //anim.SetBool("Aim",isAiming);
    }



    private void AimDownSights()
    {


        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aodSpeed);
            //isAiming = true;
            //Debug.Log("tintesc");
            FovChange.fieldOfView = Mathf.Lerp(FovChange.fieldOfView, fovAmount, Time.deltaTime * fovSmoothAmount); //fov decrease on aiming
        }
        else
        {


            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aodSpeed);
            //  isAiming = false;
            // Debug.Log("nu  tintesc");

            FovChange.fieldOfView = Mathf.Lerp(FovChange.fieldOfView, 60f, Time.deltaTime * fovSmoothAmount);// fov restored on releasing m2


        }
    }



    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
            return;

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name + " found!");

            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            hitParticleEffect.transform.SetParent(hit.transform);//face efectul de lovire copil al obiectului distrus pt a disparea odata cu obiectul 
            bulletHole.transform.SetParent(hit.transform);//face gaura ,copil al obiectului distrus pt a  disparea odata cu obiectul 


            Destroy(hitParticleEffect, 1f);
            Destroy(bulletHole, 10f);

            if (hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.01f); // Play the fire animation
        MuzzleFlash.Play(); // Shows muzzle flash when shooting
        PlayShootSound(); // Plays the shootsound effect

        currentBullets--; // Deduct one bullet
        fireTimer = 0.0f; // Resets fire timer
        UpdateAmmoText();
        UpdateFireSelectorText();


    }



    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerClip - currentBullets;
        //                           IF                     THEN   1ste     Else    2nd        
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft; // Used when having less bullets than a clip

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
        Debug.Log("Am incarcat.");

        UpdateAmmoText();
        UpdateFireSelectorText();

    }

    private void DoReload()
    {

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;

        anim.CrossFadeInFixedTime("Reload", 0.01f);
        Debug.Log("animatie de incarcare");
    }

    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);

    }

    private void UpdateAmmoText()
    {

        ammoText.text = currentBullets + " / " + bulletsLeft;



    }


    private void UpdateFireSelectorText() {


        if (semi == true)
            fireSelectorText.text = "Semi";
        else
            if (semi == false)
            fireSelectorText.text = "Auto";

    }

}
