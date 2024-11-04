using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourelles : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à tourelles
    [SerializeField] private GameObject _projectile; 
    [SerializeField] private GameObject _cible; 
    [SerializeField] private AudioClip _sonTourellesOff; 

    void Start()
    {
        // Tire chaque .5 sec
        InvokeRepeating("Tirer", 0, 0.2f);
    }

    // Tire dans la direction du joueur lorsqu'il est dans salle 1
    void Tirer()
    {
        if (_cible.transform.position.x >= 0 && _cible.transform.position.x <= 19)
        {
            float angle = TrouverAngle(transform.position, _cible.transform.position);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            Instantiate(_projectile, gameObject.transform.position, gameObject.transform.rotation);
        }
        // Si passé salle 1, désactive tourelles
        else if (_cible.transform.position.x >= 19)
        {
            SoundManager.instance.Jouer(_sonTourellesOff);
            CancelInvoke();
        }
    }

    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }
}
