using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("Interaction Values")]
    public float interactRange;

    private Vector2 currentAimDirection;

    private GameObject currentlySelected;

    public Inventory inventory { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAim();
        HandleInteraction();
    }

    void HandleInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentAimDirection, interactRange, LayerMask.GetMask("Interactable"));
        if (hit.collider != null)
        {
            currentlySelected = hit.collider.gameObject;
        }
        else
        {
            currentlySelected = null;
        }

    }

    // Currently only adds items
    public void OnInteract()
    {
        if (currentlySelected != null && currentlySelected.CompareTag("Item"))
        {
            inventory.AddItem(currentlySelected);
            currentlySelected = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentlySelected != null)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position, currentAimDirection * interactRange);
    }

    void HandleAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        currentAimDirection = (new Vector2(mousePos.x, mousePos.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
    }
}

public class Inventory
{
    List<GameObject> items;

    public Inventory()
    {
        items = new List<GameObject>();
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.SetActive(false);
        Debug.Log("Added " + item.name + " to inventory");
        Debug.Log("Current Inventory: ");
        items.ForEach(item =>
        {
            Debug.Log(item.name);
        });
    }
}