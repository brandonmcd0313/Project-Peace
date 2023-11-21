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
            // Spawn a bullet at the tip of the weapon box collider
            BoxCollider2D weaponCollider = weapon.GetComponent<BoxCollider2D>();

            // Calculate the spawn position at the tip of the weapon, considering both x and y offsets
            Vector3 spawnPosition = weaponCollider.transform.position +
                weaponCollider.transform.right * (weaponCollider.size.x / 2 + weaponCollider.offset.x) +
                weaponCollider.transform.up * weaponCollider.offset.y;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, weapon.transform.rotation);

            // Add an offset to the bullet's position to place it at the tip of the collider
            float bulletOffset = 0.5f; // Adjust this value based on your specific case

            // Set the bullet's position
            bullet.transform.position = spawnPosition;

            //add force in the direction the weapon is facing
            if (GetComponent<PlayerController>().isPlayerFacingRight())
            {
                bullet.GetComponent<Rigidbody2D>().AddForce(weapon.transform.right * bulletSpeed, ForceMode2D.Impulse);
                spawnPosition += weaponCollider.transform.right * bulletOffset;

            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().AddForce(-weapon.transform.right * bulletSpeed, ForceMode2D.Impulse);
                spawnPosition += -weaponCollider.transform.right * bulletOffset;

            }


        }
    }
}
