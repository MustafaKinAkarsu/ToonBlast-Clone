using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CellClickHandler : MonoBehaviour
{
    private AudioManager audioManager;
    public int column, row;
    Vector2 currentPosition;
    //public static Action<CellClickHandler> OnCellClick;
    public Vector2 CurrentPosition
    {
        get { return currentPosition; }
        set { currentPosition = value; }
    }

    private void OnMouseDown()
    {
        GameManager.Instance.UpdateMove();
        GridController.Instance.CheckAndDestroyChains(this.gameObject);       
    }
    //private void Update()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0); 

    //        if (touch.phase == TouchPhase.Began)
    //        {

    //            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    //            Collider2D collider = Physics2D.OverlapPoint(touchPosition);

    //            if (collider != null)
    //            {
    //                CellClickHandler cellClickHandler = collider.GetComponent<CellClickHandler>();

    //                if (cellClickHandler != null)
    //                {
    //                    //gameManager.UpdateMove();

    //                    //if (cellClickHandler.CompareTag("R_Right"))
    //                    //{
    //                    //    controller.MissileHorizontal(cellClickHandler.gameObject);
    //                    //}
    //                    //else if (cellClickHandler.CompareTag("R_Up"))
    //                    //{
    //                    //    controller.MissileVertical(cellClickHandler.gameObject);
    //                    //}
    //                    //else
    //                    //{
    //                    //    controller.CheckAndDestroyChains(cellClickHandler.gameObject);
    //                    //}
    //                }
    //            }
    //        }
    //    }
    //}
}

