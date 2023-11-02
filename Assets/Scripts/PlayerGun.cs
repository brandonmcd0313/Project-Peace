using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //on shoot button press
        if (Input.GetButtonDown("Fire1"))
        {
            
            //spawn bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //add force to bullet
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
