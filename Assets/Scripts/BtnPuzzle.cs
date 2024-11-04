using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnPuzzle : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets aux btns puzzle
    [SerializeField] private GameManager _gm;
    [SerializeField] private AudioClip _sonBtnPuzzle; // prefab du projectile

    // Déclaration des différentes propriétés
    private SpriteRenderer _sr;

    void Start()
    {
        // Assignation du SpriteRenderer dans la propriété _sr
        _sr = GetComponent<SpriteRenderer>();
    }

    // Lorsque bloc entre dans collider de bouton, joue son btn
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Perso"))
        {
            SoundManager.instance.Jouer(_sonBtnPuzzle);
        }
    }

    // Lorsque bloc sort btn, appelle fonction sur GameManager pour mettre btn dans état mal placé
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Perso"))
        {
            _sr.color = new Color(1f, 1f, 1f, 1f);
            _gm.BlocSort(gameObject.tag);
        }
    }

    // Tant que bloc reste sur btn, vérifie si se trouve sur bon btn
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Perso"))
        {
            _sr.color = new Color(.5f, .5f, .5f, 1f);

            _gm.VerifierCode(col.tag, gameObject.tag);
        }
    }
}

