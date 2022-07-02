using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageViewer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private string _template;
    [SerializeField] private float _disappearanceTime;

    private void Awake()
    {
        Fighter fighter = GetComponent<Fighter>();
        fighter.OnDamageReceived += (_, damage) => SetDamageText(damage);  
    }

    private void SetDamageText(float damage)
    {
        _text.text = string.Format(_template, damage);
        Appear();
        StartCoroutine(Disappear());
        //_text.CrossFadeAlpha(1f, 1f, false);
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f);
        Color currColor = _text.color;
        currColor.a = 0f;
        _text.color = currColor;
    }

    private void Appear()
    {
        Color currColor = _text.color;
        currColor.a = 1f;
        _text.color = currColor;
    }

    //private IEnumerator TextFadeCoroutine(float speed)
    //{
    //    Color currColor;
    //    float timer = 0f;

    //    do
    //    {
    //        timer += Time.deltaTime;
    //        currColor = _text.color;
    //        currColor.a -= speed;
    //        _text.color = currColor;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    while (currColor.a > 0);
    //}
}
