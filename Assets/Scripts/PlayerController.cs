using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    static public PlayerController S;
    public float health;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _barrel;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 30f;
    private float _moveDirection;
    private float _turnDirection;
   

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }
    
    void Start()
    {
        ResetHealth();
    }

    void Update()
    {        
        _moveDirection = Input.GetAxis("Vertical");
        _turnDirection = Input.GetAxis("Horizontal");
        if (_moveDirection != 0)
        {
            Move(_moveDirection);
            if (_moveDirection > 0)
                Turn(_turnDirection);
            else
                Turn(-_turnDirection);
        }
        if (Input.GetButtonDown("Jump"))
            Fire();
        LimitPosition();
    }

    void OnTriggerEnter(Collider other)
    {
        CameraShake.S.Shake();
        Bullet bullet = other.GetComponent<Bullet>();
        TakeDamage(10f);
        if (health > 0)
            bullet.PlayExplosionEffect(ExplosionType.Small);
        else
        {
            bullet.PlayExplosionEffect(ExplosionType.Big);
            Die();
        }

    }   

    void Move(float direction)
    {
        transform.Translate(Vector3.forward * direction * Time.deltaTime * _speed);
    }

    void Turn(float turnDirection)
    {
        transform.Rotate(Vector3.up * turnDirection * Time.deltaTime * _turnSpeed);
    }

    void Fire()
    {
        Vector3 wantedRotation = new Vector3(90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        Instantiate(_bulletPrefab, _barrel.position, Quaternion.Euler(wantedRotation));
    }

    void LimitPosition()
    {
        if (transform.position.x > 11)
            transform.position = new Vector3(11f, 1.2f, transform.position.z);
        else if (transform.position.x < -11f)
            transform.position = new Vector3(-11f, 1.2f, transform.position.z);
        if(transform.position.z > 20f)
            transform.position = new Vector3(transform.position.x, 1.2f, 20f);
        else if (transform.position.z < -20f)
            transform.position = new Vector3(transform.position.x, 1.2f, -20f);
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0f)
        {
            health = 0f;            
        }
    }

    public void ResetHealth()
    {
        health = 100f;           
    }
    
    public void Die()
    {        
        Destroy(gameObject, 0.5f);
    }

    
}
