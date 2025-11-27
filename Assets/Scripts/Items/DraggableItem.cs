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
    Camera cameraC;
    Vector3 originalPosition;
    public Transform originalParent;

    public delegate void OnItemBeginDrag(UIInventoryItem uiItem);
    public static event OnItemBeginDrag onItemBeginDrag;

    public delegate void OnItemEndDrag();
    public static event OnItemEndDrag onItemEndDrag;

    private void Start()
    {
        cameraC = Camera.main;
        originalLayer = this.gameObject.layer;
        image = this.gameObject.GetComponent<Image>();
        originalPosition = transform.position;
        originalParent = transform.parent;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start parent " + originalParent.gameObject.name);
        if (this.isActiveAndEnabled)
        {

            this.gameObject.layer = LayerMask.NameToLayer("DraggableItem");
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            //Pour cacher l'image le temps que le pointeur sache si on est sur une case de Drop
            image.raycastTarget = false;

            if (originalParent.GetComponent<UIInventoryItem>() != null)
            {
                onItemBeginDrag?.Invoke(originalParent.GetComponent<UIInventoryItem>());
            }
            else if(originalParent.GetComponent<UIEquipmentItem>() != null)
            {
               /* onItemBeginDrag?.Invoke((originalParent.GetComponent<UIEquipmentItem>());*/
            }

        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = cameraC.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y , cameraC.nearClipPlane));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        Debug.Log("end parent "+ originalParent.gameObject.name);
        transform.SetParent(originalParent);
        this.gameObject.layer = originalLayer;
        transform.position = originalPosition;

        if(image.sprite == null)
        {
            image.gameObject.SetActive(false);
        }

        onItemEndDrag?.Invoke();
    }

    
}

