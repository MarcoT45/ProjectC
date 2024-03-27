using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector]

    LayerMask originalLayer;
    Camera camera;
    Vector3 originalPosition;
    Transform originalParent;

    public delegate void OnItemBeginDrag(UIInventoryItem uiItem);
    public static event OnItemBeginDrag onItemBeginDrag;

    public delegate void OnItemEndDrag();
    public static event OnItemEndDrag onItemEndDrag;

    private void Start()
    {
        camera = Camera.main;
        originalLayer = this.gameObject.layer;
        image = this.gameObject.GetComponent<Image>();
        originalPosition = transform.position;
        originalParent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(this.isActiveAndEnabled)
        {
            onItemBeginDrag?.Invoke(this.GetComponentInParent<UIInventoryItem>());

            this.gameObject.layer = LayerMask.NameToLayer("DraggableItem");
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            //Pour cacher l'image le temps que le pointeur sache si on est sur une case de Drop
            image.raycastTarget = false;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y , camera.nearClipPlane));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        this.gameObject.layer = originalLayer;
        transform.position = originalPosition;
        image.raycastTarget = true;

        onItemEndDrag?.Invoke();
    }

    
}

