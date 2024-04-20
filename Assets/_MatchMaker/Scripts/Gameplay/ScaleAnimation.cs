using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScaleAnimation : MonoBehaviour
{
    private Vector3 _startScale;

    [SerializeField] private float _magnitude;

    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = _startScale * (0.8f + Mathf.PingPong(Time.time * _speed, _magnitude));
    }
    private void OnDisable()
    {
        transform.localScale = _startScale;
    }
}

