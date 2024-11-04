using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à Projectile
    [SerializeField] private GameObject _particules;
    [SerializeField] private AudioClip _sonTir;
    [SerializeField] private AudioClip _sonDestruction;

    // Déclaration des différentes propriétés
    private float _vitesse = 20f;
    void Start()
    {
        // Joue son
        SoundManager.instance.Jouer(_sonTir, .3f);
    }
    void Update()
    {
        // Le projectile avance selon sa vitesse dans la direction
        // de son propre axe horizontal (Space.Self)
        transform.Translate(Vector3.right * _vitesse * Time.deltaTime, Space.Self);
    }

    // Quand collision avec tout sauf fondBoss, joue son, déclanche particules puis autodestruction
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("fondBoss"))
        {
            SoundManager.instance.Jouer(_sonDestruction, .5f);
            Instantiate(_particules, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
