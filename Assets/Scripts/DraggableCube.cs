using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class DraggableCube : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private Vector3 offset;
    private float distanceToCamera;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, distanceToCamera));
        transform.position = newPosition + offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
        Debug.Log(GetComponent<NetworkObject>().IsOwner);
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, distanceToCamera));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<NetworkObject>().RemoveOwnership();
    }
}
