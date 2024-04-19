using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]private Sprite _frontSprite;
    [SerializeField]private Sprite _backSprite;
    [SerializeField]private float _flipDuration =0.5f;
    [SerializeField]private AnimationCurve _flipInAnimationCurve;
    [SerializeField]private AnimationCurve _flipOutAnimationCurve;
    private Image _image;
    private bool _flipped = false;
    private readonly Color TransparentColor = new Color(1, 1, 1, 0);
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    public void Init(Sprite frontSprite, Vector2 size, Vector2 position)
    {
        _frontSprite = frontSprite;
        _image.rectTransform.sizeDelta = size;
        _image.transform.localPosition = position;
        if(!_backSprite)
            _image.color = TransparentColor;
        gameObject.SetActive(true);
    }
    [ContextMenu("Flip")]
    public void Flip()
    {
        StartCoroutine(FlipAnimation());
    }
    private IEnumerator FlipAnimation()
    {
        
        float startTime = Time.time;
        Vector3 currentRotation = transform.eulerAngles;
        float rotationY = currentRotation.y;
        if (_flipped)
        {
            while (rotationY > 0f)
            {
                rotationY = Mathf.Lerp(rotationY, 0, _flipOutAnimationCurve.Evaluate( (Time.time - startTime) / _flipDuration));
                currentRotation.y = rotationY;
                transform.eulerAngles = currentRotation;
                if (rotationY < 90f && _image.sprite != _backSprite)
                {
                    _image.sprite = _backSprite;
                }
                yield return null;
            }
            _flipped = false;
        }
        else
        {
            while (rotationY < 180f)
            {
                rotationY = Mathf.Lerp(rotationY, 180,_flipInAnimationCurve.Evaluate(_flipInAnimationCurve.Evaluate(Time.time - startTime) / _flipDuration));
                currentRotation.y = rotationY;
                transform.eulerAngles = currentRotation;
                if (rotationY > 90f && _image.sprite != _frontSprite)
                {
                    _image.sprite = _frontSprite;
                }
                yield return null;
            }
            _flipped = true;
        }
        
    }
}
