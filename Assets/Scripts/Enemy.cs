using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;


public class Enemy : MonoBehaviour {

    static public Enemy S;
    public float health;
    private NavMeshAgent _navMeshAgent;
    private bool isFiring = false;
    
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _barrel;
    [SerializeField] private Transform _player;
    [SerializeField] private Image _lifeSlider;
    

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        ResetHealth();
        _player = FindObjectOfType<PlayerController>().transform;
        StartCoroutine(Move());
        StartCoroutine(Fire());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 wantedRotation = new Vector3(90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            Instantiate(_bulletPrefab, _barrel.position, Quaternion.Euler(wantedRotation));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CameraShake.S.Shake();
        Bullet bullet = other.GetComponent<Bullet>();
        TakeDamage(20f);
        if (health > 0)
            bullet.PlayExplosionEffect(ExplosionType.Small);
        else
        {
            bullet.PlayExplosionEffect(ExplosionType.Big);
            Die();
        }

    }

    public IEnumerator Move()
    {
        while (true)
        {
            if (!isFiring)
            {                
                Vector3 destination = new Vector3(Random.Range(-11f, 11f), 1.25f, Random.Range(-20f, 20f));
                _navMeshAgent.SetDestination(destination);
                yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));

            }
            yield return null;
        }        
    }

    public IEnumerator Fire()
    {
        while (true) // change to while(isAlive), also in PlayerController
        {            
            yield return new WaitForSeconds(Random.Range(2.5f, 3.5f));
            isFiring = true;
            yield return new WaitUntil(() => _navMeshAgent.velocity == Vector3.zero);
            if (_player)
                transform.DOLookAt(_player.position, 0.75f);
            yield return new WaitUntil(() => !DOTween.IsTweening(transform));
            Vector3 wantedRotation = new Vector3(90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            Instantiate(_bulletPrefab, _barrel.position, Quaternion.Euler(wantedRotation));
            yield return new WaitForEndOfFrame();
            isFiring = false;
        }
        
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0f)
        {
            health = 0f;            
        }
    }

    public void Die()
    {       
        Destroy(gameObject);
    }

    public void ResetHealth()
    {
        health = 100f;
    }

    void OnDestroy()
    {
        GameManager.S.UpdateKills();
        GameManager.S.UpdateLevel();
        
    }
}
