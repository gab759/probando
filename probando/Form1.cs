using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private HealthBarController playerHealthBar;

    private void Start()
    {
        if (playerHealthBar == null)
        {
            playerHealthBar = FindObjectOfType<HealthBarController>();
        }
    }
    private void Update()
    {
        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        Vector2 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);

        Debug.DrawRay(transform.position, mouseInput.normalized * rayDistance, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet(mouseInput);
            myAnimator.SetTrigger("GoAttack");
            Debug.Log("Right Click");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ShootBullet(mouseInput);
            myAnimator.SetTrigger("GoAttack");
            Debug.Log("Left Click");
        }
    }
    public void OnMovemet(InputAction.CallbackContext contex)
    {
        Vector2 movementPlayer = contex.ReadValue<Vector2>();
        myRBD2.velocity = movementPlayer * velocityModifier;
    }
    private void CheckFlip(float x_Position)
    {
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
    private void ShootBullet(Vector2 mousePosition)
    {
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogError("El prefab de la bala no tiene un Rigidbody2D adjunto.");
        }
    }
    public void TakeDamage(int amount)
    {
        playerHealthBar.UpdateHealth(-amount);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy1"))
        {
            if (playerHealthBar != null)
            {
                playerHealthBar.UpdateHealth(20);
            }
        }
    }
}
