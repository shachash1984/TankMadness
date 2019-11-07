using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosionType { Small, Big }

public class Bullet : MonoBehaviour {

    [SerializeField] private float _bulletSpeed = 30f;
    private ExplosionType _explosionType;
    [SerializeField] private GameObject _smallExplosion;
    [SerializeField] private GameObject _bigExplosion;
    [SerializeField] private Renderer _bulletMesh;
    [SerializeField] private Renderer _shellMesh;
    private bool hitTarget = false;

    

    void Update()
    {
        if (!hitTarget)
            Move();
    }

    void Move()
    {        
        transform.Translate(-transform.forward * Time.deltaTime * _bulletSpeed);        
    }

    public void PlayExplosionEffect(ExplosionType ext)
    {
        StartCoroutine(PlayExplosion(ext));
    }

    private IEnumerator PlayExplosion(ExplosionType ext)
    {
        hitTarget = true;
        _bulletMesh.enabled = false;
        _shellMesh.enabled = false;
        switch (ext)
        {
            case ExplosionType.Small:
                _smallExplosion.SetActive(true);
                yield return new WaitForSeconds(2f);
                _smallExplosion.SetActive(false);
                break;
            case ExplosionType.Big:
                _bigExplosion.SetActive(true);
                yield return new WaitForSeconds(3f);
                _bigExplosion.SetActive(false);
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }

    

    
}
