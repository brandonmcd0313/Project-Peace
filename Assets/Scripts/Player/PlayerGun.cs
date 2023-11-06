using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;

    [Header("Weapon")]
    [SerializeField] GameObject weapon;
    [Tooltip("The angle range the weapon can rotate to")]
    [SerializeField] Vector2 angleRange; 
    
   
    // Update is called once per frame
    void Update()
    {
        // Rotate weapon to face the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 directionToMouse = (mousePosition - weapon.transform.position).normalized;
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
       
        //translate angle to be between -180 and 180
        if(angleToMouse >= 90)
        {
            angleToMouse -= 180;
        }
        else if(angleToMouse < -90)
        {
            angleToMouse += 180;
        }
        angleToMouse = Mathf.Clamp(angleToMouse, angleRange.x, angleRange.y);
        // Set the rotation of the weapon based on the clamped angle
        weapon.transform.rotation = Quaternion.Euler(0, 0, angleToMouse);

        
        //on shoot button press
        if (Input.GetButtonDown("Fire1"))
        {
            //spawn a bullet at the tip of the weapon
            Vector3 spawnPosition = new Vector3(weapon.transform.position.x + (weapon.GetComponent<BoxCollider2D>().size.x / 2) + (bulletPrefab.GetComponent<BoxCollider2D>().size.x /2),
                weapon.transform.position.y, weapon.transform.position.z);
            GameObject bullet = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation);
            bullet.transform.rotation = weapon.transform.rotation;
            //add force in the direction the weapon is facing
            if(GetComponent<PlayerController>().isPlayerFacingRight())
            {
                bullet.GetComponent<Rigidbody2D>().AddForce(weapon.transform.right * bulletSpeed, ForceMode2D.Impulse);
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().AddForce(-weapon.transform.right * bulletSpeed, ForceMode2D.Impulse);
            }

        }
    }
}
