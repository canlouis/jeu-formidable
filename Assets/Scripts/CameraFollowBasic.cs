using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBasic : MonoBehaviour
{
    [SerializeField] private Transform _cible;
    [SerializeField] private float _vitesse;

    private Vector3 _startPos;
    private Vector3 _endPos;

    void Update()
    {
        _startPos = transform.position;
        _endPos = _cible.position;
        _endPos.z = transform.position.z; 
         transform.position = Vector3.Lerp(_startPos, _endPos, _vitesse * Time.deltaTime);  
    }
}