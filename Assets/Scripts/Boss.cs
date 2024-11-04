using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à Boss
    [SerializeField] private Sprite[] _barreTab;
    [SerializeField] private GameObject _barreVie;
    [SerializeField] private GameObject _projectile; 
    [SerializeField] private GameObject _canon; 
    [SerializeField] private GameObject _bout; 
    [SerializeField] private GameObject _mort; 
    [SerializeField] private GameObject _perso; 
    [SerializeField] private GameManager _gM; 
    [SerializeField] private AudioClip _sonMort; 
    [SerializeField] private AudioClip _sonDegat; 


    // Déclaration des différentes propriétés
    private SpriteRenderer _sr;
    private Vector3 _direction;
    private int _vies = 15;
    private int _vitesse = 1;
    private float _couleurDegat = .08f;
    float _couleur = .9f;

    // Start is called before the first frame update
    void Start()
    {
        // Tire chaque .5 sec
        InvokeRepeating("Tirer", 0, 0.5f);

        // Assignation SpriteRender dans propriété
        _sr = GetComponent<SpriteRenderer>();
        // Désactive barre vie Boss
        _barreVie.SetActive(false);
    }

    void Update()
    {
        // Cherche angle entre Boss et Perso
        float angle = TrouverAngle(transform.position, _perso.transform.position);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        _canon.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        SeDeplacer();
    }

    public void PerdreVie()
    {
        SoundManager.instance.Jouer(_sonDegat);
        // Change couleur vaisseau montrant dégat
        _sr.color = new Color(_couleur, _couleur, _couleur, 1f);
        // 1 vie perdue
        _vies--;
        // Change couleur selon dégat subits
        _couleur -= _couleurDegat;

        // Appel de fonction permettant de faire baisser barre vie
        ChangerBarreVie();

        // Si plus de vies (mort)...
        if (_vies == 0)
        {
            // Apparition explosion
            Instantiate(_mort, transform.position, Quaternion.identity);
            // Joue _sonMort sur _gameManager
            SoundManager.instance.Jouer(_sonMort);
            // Appelle fonction Victoire sur _gameManager
            _gM.Invoke("Victoire", 2f);
            // Autodestruction
            Destroy(gameObject);
        }
    }
    public void ChangerBarreVie()
    {
        // Change sprite barre vie selon nb vies restantes
        _barreVie.GetComponent<SpriteRenderer>().sprite = _barreTab[_vies];
    }

    // Fait perdre vie lorsque touché par projectile
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Si collision avec projectile
        if (col.CompareTag("Tir"))
        {
            PerdreVie();
        }
    }

    // Fait tirer Boss quand Perso est dans 3e salle
    void Tirer()
    {
        if (_perso.transform.position.x >= 61 && _perso.transform.position.x <= 102)
        {
            Instantiate(_projectile, _bout.transform.position, _canon.transform.rotation);
        }
    }

    // Cherche agngle entre 2 objets donnés
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    // Déplace Boss vers Perso jusqu'à ce que distance entre eux >= 4 unités
    private void SeDeplacer()
    {
        if (_perso.transform.position.x >= 61 && _perso.transform.position.x <= 102)
        {
            // Réactive barre vie Boss
            _barreVie.SetActive(true);
            _direction = (_perso.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(_perso.transform.position, transform.position);
            if (distance >= 4 || distance <= -4)
            {
                transform.Translate(_direction * _vitesse * Time.deltaTime, Space.World);
            }
        }

    }
}
