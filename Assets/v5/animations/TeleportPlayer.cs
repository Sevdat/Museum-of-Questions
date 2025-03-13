using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject portal1; // First portal (child of linkedPortals)
    public GameObject portal2; // Second portal (child of linkedPortals)

    private void Start()
    {
        // Ensure portal1 and portal2 are assigned
        if (portal1 == null || portal2 == null)
        {
            // Debug.LogError("Portals are not assigned in the Inspector!");
            return;
        }

        // Add trigger event listeners to the portals
        AddTriggerListener(portal1);
        AddTriggerListener(portal2);
    }

    private void AddTriggerListener(GameObject portal)
    {
        // Get or add a TriggerHandler component to the portal
        TriggerHandler triggerHandler = portal.GetComponent<TriggerHandler>();
        if (triggerHandler == null)
        {
            triggerHandler = portal.AddComponent<TriggerHandler>();
        }

        // Subscribe to the trigger event
        triggerHandler.OnTriggerEnterEvent += HandleTriggerEnter;
    }

    private void HandleTriggerEnter(Collider other, GameObject portal)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger: " + portal.name);

            // Teleport the player to the other portal
            if (portal == portal1)
            {
                TeleportObject(other.gameObject, portal2);
            }
            else if (portal == portal2)
            {
                TeleportObject(other.gameObject, portal1);
            }
        }
    }

    private void TeleportObject(GameObject objectToTeleport, GameObject destinationPortal)
    {
        // Teleport the object to the destination portal's position
        objectToTeleport.transform.position = destinationPortal.transform.position + objectToTeleport.transform.forward*2;

        // Optional: Adjust rotation to match the destination portal's rotation
        objectToTeleport.transform.rotation = destinationPortal.transform.rotation;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the trigger events to avoid memory leaks
        if (portal1 != null)
        {
            TriggerHandler triggerHandler = portal1.GetComponent<TriggerHandler>();
            if (triggerHandler != null)
            {
                triggerHandler.OnTriggerEnterEvent -= HandleTriggerEnter;
            }
        }

        if (portal2 != null)
        {
            TriggerHandler triggerHandler = portal2.GetComponent<TriggerHandler>();
            if (triggerHandler != null)
            {
                triggerHandler.OnTriggerEnterEvent -= HandleTriggerEnter;
            }
        }
    }
}

// Helper script to handle trigger events on the portals
public class TriggerHandler : MonoBehaviour
{
    public System.Action<Collider, GameObject> OnTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        // Invoke the event and pass the collider and the portal GameObject
        OnTriggerEnterEvent?.Invoke(other, gameObject);
    }
}