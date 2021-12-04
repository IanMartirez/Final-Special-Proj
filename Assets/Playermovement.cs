using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Playermovement : MonoBehaviour
{

    public float MoveDuration = 2f;
    private  Animator m_Animator;

    private Camera m_Cam;
    private Vector3 m_Point;

    private bool m_IsHorizontal = false;

    private Vector3 m_DummyMousePos;

    // Declares variables needed
    private void OnValidate()
    {
        m_Cam = Camera.main;
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // Kill all DOTweens
            transform.DOKill();

            // Starts moving animation
            m_Animator.SetTrigger("Move");

            m_DummyMousePos.x = Input.mousePosition.x;
            m_DummyMousePos.y = Input.mousePosition.y;
            m_DummyMousePos.z = m_Cam.nearClipPlane;

            m_Point = m_Cam.ScreenToWorldPoint(m_DummyMousePos);

            // Randomizes between doing horizontal or vertical movement first
            if (Random.value < .5f) m_IsHorizontal = false;
            else m_IsHorizontal = true;

            // Reset Blend Tree Values
            m_Animator.SetFloat("Vertical", 0);
            m_Animator.SetFloat("Horizontal", 0);

            if (m_IsHorizontal)
            {
                // Moves in X
                float m_Speed = Mathf.Abs(m_Point.x - transform.position.x) / MoveDuration;
                m_Animator.SetFloat("Horizontal", m_Point.x - transform.position.x);

                transform.DOMoveX(m_Point.x, m_Speed).OnComplete(() =>
                {
                    // Moves in Y
                    m_Speed = Mathf.Abs(m_Point.y - transform.position.y) / MoveDuration;
                    transform.DOMoveY(m_Point.y, m_Speed).SetEase(Ease.Linear);

                    m_Animator.SetFloat("Vertical", m_Point.y - transform.position.y);
                    m_Animator.SetFloat("Horizontal", 0);
                    m_Animator.SetTrigger("Move");
                }).SetEase(Ease.Linear);
            }
            else
            {
                // Moves in Y
                float m_Speed = Mathf.Abs(m_Point.y - transform.position.y) / MoveDuration;
                m_Animator.SetFloat("Vertical", m_Point.y - transform.position.y);

                transform.DOMoveY(m_Point.y, m_Speed).OnComplete(() =>
                {
                    // Moves in X
                    m_Speed = Mathf.Abs(m_Point.x - transform.position.x) / MoveDuration;
                    transform.DOMoveX(m_Point.x, m_Speed).SetEase(Ease.Linear);

                    m_Animator.SetFloat("Horizontal", m_Point.x - transform.position.x);
                    m_Animator.SetFloat("Vertical", 0);
                    m_Animator.SetTrigger("Move");
                }).SetEase(Ease.Linear);
            }
        }
    }
}
