using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à ProjectileBoss
    [SerializeField] private GameObject _particules; 
    [SerializeField] private AudioClip _sonTir; 
    [SerializeField] private AudioClip _sonDestruction; 

    // Déclaration des différentes propriétés
    private Rigidbody2D _rb;
    private float _vitesse = 6f;

    void Start()
    {
        // Assignation RigidBody2D dans propriété
        _rb = GetComponent<Rigidbody2D>();
        // Joue son
        SoundManager.instance.Jouer(_sonTir, .8f);
    }

    // Quand collision, joue son, déclanche particules puis autodestruction
    void OnTriggerEnter2D(Collider2D col)
    {
        SoundManager.instance.Jouer(_sonDestruction, 1);
        Instantiate(_particules, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Déplacement projectile Boss constant avec FixedUpdate
    void FixedUpdate()
    {
        _rb.MovePosition(transform.position * _vitesse * Time.fixedDeltaTime);
    }
}
