using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Perso : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à Perso
    [SerializeField] private Sprite[] _barreTab;
    [SerializeField] private GameObject _barreVie;
    [SerializeField] private GameObject _projectile; 
    [SerializeField] private GameObject _canon; 
    [SerializeField] private GameObject _bout; 
    [SerializeField] private GameObject _portail;
    [SerializeField] private GameObject _mort; 
    [SerializeField] private GameManager _gM; 
    [SerializeField] private AudioClip _sonMort; 
    [SerializeField] private AudioClip _sonDegat; 
    [SerializeField] private AudioClip _sonRamasserCanon; 
    [SerializeField] private AudioClip _sonPortail; 

    // Déclaration des différentes propriétés
    private SpriteRenderer _sr;
    private Vector3 _axis;
    private Rigidbody2D _rb;
    private int _vies = 2;
    private int _vitesse = 5;
    private bool _vulnerable = true;
    private bool _peutTirer = false;
    void Start()
    {
        // Assignation de certaines propriétés avant composant Perso correspondant
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        // Canon invisible
        _canon.GetComponent<SpriteRenderer>().enabled = false;
        // Désactive portail
        _portail.SetActive(false);
    }

    void Update()
    {
        // Détermine l'axe du bouton appuyé pour se déplacé (wasd / flèches)
        _axis.x = Input.GetAxisRaw("Horizontal");
        _axis.y = Input.GetAxisRaw("Vertical");
        _axis.Normalize();

        // Détermine si Perso a récupéré canon
        if (_peutTirer)
        {
            // Si oui, tir en cliquant dans la direction de la souris
            if (Input.GetMouseButtonDown(0))
            {
                Tirer();
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = TrouverAngle(transform.position, mousePos);
            _canon.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }

        Teleporter();
    }

    // Déplacement Perso
    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + _axis * _vitesse * Time.fixedDeltaTime);
    }

    // Se téléporte dans salle 1, 2 ou 3ven appuyant sur 1, 2 et 3
    void Teleporter()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = new Vector3(-3, .5f, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = new Vector3(28, .5f, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.position = new Vector3(77, -2.5f, 0);
            _canon.GetComponent<SpriteRenderer>().enabled = true;
            _peutTirer = true;
        }
    }

    public void PerdreVie()
    {
        // Fait jouer le son _sonDegat
        SoundManager.instance.Jouer(_sonDegat, 2);
        // Rend Perso invincible
        _vulnerable = false;
        // Change couleur vaisseau montrant dégat
        _sr.color = new Color(.5f, .5f, .5f, 1f);
        // 1 vie perdue
        _vies--;

        // Appel de fonction permettant de faire baisser barre vie
        ChangerBarreVie();

        // Si plus de vies (mort)...
        if (_vies == 0)
        {
            // Apparition explosion
            Instantiate(_mort, transform.position, Quaternion.identity);
            // Joue _sonMort sur _gameManager
            SoundManager.instance.Jouer(_sonMort);
            // _gameManager.JouerSon();
            // Appelle fonction GameOver sur _gameManager
            _gM.Invoke("GameOver", 1.5f);
            // Fait disparaître perso
            gameObject.SetActive(false);
        }

        // Appelle fonction FinInvincibilite après 2s
        Invoke("FinInvincibilite", 1.5f);
    }

    public void ChangerBarreVie()
    {
        // Change sprite barre vie selon nb vies restantes
        _barreVie.GetComponent<SpriteRenderer>().sprite = _barreTab[_vies];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Fait perdre vie lorsque touché par projectile
        if (col.CompareTag("Tir"))
        {
            if (_vulnerable == true)
            {
                PerdreVie();
            }
        }

        // Téléporte perso dans 3e salle lorsque touche portail
        if (col.CompareTag("Portail"))
        {
            SoundManager.instance.Jouer(_sonPortail, 10);
            transform.position = new Vector3(77, -2.5f, 0);
        }

        // Permet à jouer tirer lorsqu'entre en contact avec canon
        if (col.CompareTag("CanonTrouve"))
        {
            SoundManager.instance.Jouer(_sonRamasserCanon, 3);
            PermettreTirer();
            Destroy(col.gameObject);
        }

        // Active compte à rebourt lorsqu'arrive dans salle 2
        if (col.CompareTag("Puzzle"))
        {
            col.GetComponent<BoxCollider2D>().enabled = false;
            _gM.InvokeRepeating("CompteARebourt", 0, 1f);
        }
    }

    // Déclaration fonction rendant vulnérabilité Perso
    private void FinInvincibilite()
    {
        _vulnerable = true;
        // Rend couleur d'origine Perso
        _sr.color = new Color(.9f, .9f, .9f, 1f);
    }

    // Fait tirer perso vers souris lorsqu'appuyée
    void Tirer()
    {
        Instantiate(_projectile, _bout.transform.position, _canon.transform.rotation);
    }

    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    // Détermine si joueur a trouvé canon pour pouvoir tirer
    public void PermettreTirer()
    {
        _canon.GetComponent<SpriteRenderer>().enabled = true;
        _peutTirer = true;
        _portail.SetActive(true);
    }
}
