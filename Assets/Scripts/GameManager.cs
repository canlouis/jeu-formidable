using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Création d'espaces dans l'inspecteur pour lier différents objets à Game Manager
    [SerializeField] private GameObject[] _BlocsTab;
    [SerializeField] private GameObject[] _codeTab;
    [SerializeField] private GameObject _porte;
    [SerializeField] private GameObject _canonTrouve;
    [SerializeField] private Text _champTemps;
    [SerializeField] private AudioClip _sonMelangePuzzle;
    [SerializeField] private AudioClip _sonVictoirePuzzle;

    // Déclaration des différentes propriétés
    private List<char> _couleursList = new List<char>();
    private char[] _couleursTab = { 'R', 'V', 'B' };
    private char[] _btnValeur = { 'R', 'V', 'B' };
    private bool[] _bienPlaceTab = { false, false, false };
    private int _tempsRestant = 25;
    private bool _premiereFois = true;

    private void Start()
    {
        // Crée combinaison aléatoire puzzle dès début en faisant apparaître blocs aléatoirement dans salle 2
        CreerCode();
        ApparitionBlocsAlea();
        // Désactive canon au sol
        _canonTrouve.SetActive(false);
    }

    private void Update()
    {
        // Recommence partie si appuie sur escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Jeu");
        }
    }

    // Crée combinaison puzzle
    private void CreerCode()
    {
        RemplirListe();
        // Donne valeur aléatoire entre rouge, vert, bleu à code et associe bouton correspondant à code
        for (int i = 0; i < _codeTab.Length; i++)
        {
            // Choisi couleur aléatoire dans liste
            int clrRand = Random.Range(0, _couleursList.Count);
            char couleur = _couleursList[clrRand];
            int R = 0, V = 0, B = 0;
            // Attribue couleur aléatoire à boutons et à code
            if (couleur == 'R')
            {
                R = 1;
                _btnValeur[i] = 'R';
            }
            else if (couleur == 'B')
            {
                V = 1;
                _btnValeur[i] = 'V';
            }
            else
            {
                _btnValeur[i] = 'B';
                B = 1;
            }
            // Change couleur carrés code selon valeur attribuée
            _codeTab[i].GetComponent<SpriteRenderer>().color = new Color(R, V, B, 1f);
            // Enlève couleur aléatoire sélectionnée pour ne pas la reprendre
            _couleursList.RemoveAt(clrRand);
        }
    }

    // Vérifie si emplacement blocs correspond à code
    public void VerifierCode(string colTag, string btnTag)
    {
        for (int i = 0; i < _codeTab.Length; i++)
        {
            // Vérifie quel bouton a été appuyé
            if (btnTag == "btn" + i)
            {
                // Vérifie que bloc ayant appuyé est bon
                if (_btnValeur[i] == 'R')
                {
                    if (colTag == "BR")
                    {
                        _bienPlaceTab[i] = true;
                    }
                }
                else if (_btnValeur[i] == 'V')
                {
                    if (colTag == "BV")
                    {
                        _bienPlaceTab[i] = true;
                    }
                }
                else if (_btnValeur[i] == 'B')
                {
                    if (colTag == "BB")
                    {
                        _bienPlaceTab[i] = true;
                    }
                }
            }
        }

        // Si tous blocs bien placés...
        if (_bienPlaceTab[0] && _bienPlaceTab[1] && _bienPlaceTab[2])
        {
            // Vérifie si première fois que blocs bien placés pour effectué ci-dessous une seule fois
            if (_premiereFois)
            {
                SoundManager.instance.Jouer(_sonVictoirePuzzle);
                // Arrête compte à rebourt avant nouveau code
                CancelInvoke();
                // Fait disparaître porte
                Destroy(_porte);
                // Fait apparaître canon au sol
                _canonTrouve.SetActive(true);
                // Montre satut du puzzle au joueur
                _champTemps.text = "STATUT: RÉUSSITE";
                // Empêche code ci-haut de se répèter
                _premiereFois = false;
            }
        }
    }

    // Lorsque blocs n'appuie plus sur btn, bloc n'est plus bien placé
    public void BlocSort(string tagBtn)
    {
        for (int i = 0; i < _codeTab.Length; i++)
        {
            if (tagBtn == "btn" + i)
            {
                _bienPlaceTab[i] = false;
            }
        }
    }

    // Remplie liste couleur avec couleurs tableau
    private void RemplirListe()
    {
        for (int i = 0; i < _couleursTab.Length; i++)
        {
            _couleursList.Add(_couleursTab[i]);
        }
    }

    // Compte à rebourt avant nouvelle combinaison puzzle
    public void CompteARebourt()
    {
        // Après 25 sec, nouvelle combinaison puzzle
        if (_tempsRestant <= 0)
        {
            SoundManager.instance.Jouer(_sonMelangePuzzle, 4);
            // Recrée nouveau code et redéplace blocs à endroit aléatoire dans salle 2
            CreerCode();
            ApparitionBlocsAlea();
            _tempsRestant = 25;
        }
        // Montre temps restant avant renouvellement combinaison
        _champTemps.text = "STATUT: EN COURS\nTemps avant mélange du code: " + _tempsRestant;
        _tempsRestant--;
    }

    // Fait apparaitre blocs à emplacement aléatoire dans salle 2
    public void ApparitionBlocsAlea()
    {
        for (int i = 0; i < _BlocsTab.Length; i++)
        {
            float randX = Random.Range(28f, 39f);
            float randY = Random.Range(-5f, 6f);
            _BlocsTab[i].transform.position = new Vector3(randX, randY, 0);
            _BlocsTab[i].transform.rotation = Quaternion.identity;
        }
    }

    // Recommence partie lorsque mort
    public void GameOver()
    {
        SceneManager.LoadScene("Jeu");
    }

    // Amène à écran victoire lorsque Boss vaincu
    public void Victoire()
    {
        SceneManager.LoadScene("Fin");
    }
}