using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class CubeController : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 offset;
    private float distanceToCamera;

    public void OnDrag(PointerEventData eventData)
    {
        Dragging(eventData.position);
        if (IsServer)
            OnDraggingClientRPC(eventData.position);
        if (IsClient)
            OnDraggingServerRpc(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrag(eventData.position);
        if (IsServer)
            OnStartDragClientRPC(eventData.position);
        if (IsClient)
            OnStartDragServerRpc(eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter(); // do in local

        // do on remote
        if (IsServer)
            OnEnterClientRPC();
        if (IsClient)
            OnEnterServerRPC();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();

        // do on remote
        if (IsServer)
            OnExitClientRPC();

        if (IsClient)
            OnExitServerRPC();

    }

    #region remote control

    [ServerRpc(RequireOwnership = false)]
    void OnEnterServerRPC()
    {
        OnEnter();
    }

    [ServerRpc(RequireOwnership = false)]
    void OnExitServerRPC()
    {
        OnExit();
    }

    [ServerRpc(RequireOwnership = false)]
    void OnStartDragServerRpc(Vector2 data)
    {
        StartDrag(data);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnDraggingServerRpc(Vector2 data)
    {
        Dragging(data);
    }

    [ClientRpc]
    void OnEnterClientRPC()
    {
        OnEnter();
    }

    [ClientRpc]
    void OnExitClientRPC()
    {
        OnExit();
    }

    [ClientRpc]
    void OnStartDragClientRPC(Vector2 data)
    {
        StartDrag(data);
    }

    [ClientRpc]
    void OnDraggingClientRPC(Vector2 data)
    {
        Dragging(data);
    }

    #endregion

    #region local controller logic

    void OnEnter()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    void Dragging(Vector2 inputPos)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, distanceToCamera));
        transform.position = newPosition + offset;
    }

    void StartDrag(Vector2 inputPos)
    {
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, distanceToCamera));
    }
    #endregion

}
