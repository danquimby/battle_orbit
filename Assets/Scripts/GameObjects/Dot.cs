using System;
using UnityEngine;


public class Dot : ActorBase
{
    [SerializeField]
    private readonly Color[] colors = new Color[]{ Color.green, Color.red }; // normal / selected

    [SerializeField]
    private MotherDot motherDot;

    private float RotateSpeed = 4f;
    [SerializeField]
    private float Radius = 1.1f;

    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private Vector2 _centre;
    private float _angle;

    public bool isSelected;

    private Transform targetWayPoint;
    private bool isAttack;
    private Vector2 velocity;

    public void Init(MotherDot mother)
    {
        if (spr == null)
            spr = GetComponent<SpriteRenderer>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        motherDot = mother;
        isAttack = false;
        isSelected = false;
        _centre = motherDot.transform.position;
        UpdateColor(false);

    }
    public void Attack(Transform target)
    {
        targetWayPoint = target;
        isAttack = true;
    }
    public override void Hit()
    {
        Release();
    }
    private void Update()
    {
        if (isAttack)
        {
            Vector3 dir = (targetWayPoint.position - transform.position).normalized;
            rb.MovePosition(transform.position + dir * (5f * Time.deltaTime));

        }
        else
        {
            _angle += RotateSpeed * Time.deltaTime;
            Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;
        }
    }
    public void UpdateColor(bool selected)
    {
        isSelected = selected;
        changeColor(colors[isSelected ? 1 : 0]);
    }
    private void changeColor(Color color)
    {
        spr.color = color;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttack) return;
        string _tag = collision.gameObject.tag;
        if (_tag == motherDot.gameObject.tag) return; // наша материнка
        if (_tag == gameObject.tag) return; // наш братан

        ActorBase abstractDot = MotherDot.CastToObj<ActorBase>(collision.gameObject);
        abstractDot.Hit();
        Release(); // обьект в кого то попал нужно его удалять
    }
}