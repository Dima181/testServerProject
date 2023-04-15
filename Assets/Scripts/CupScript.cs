using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    private EventService eventService;

    private void Start()
    {
        eventService = FindObjectOfType<EventService>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eventService.TrackEvent("CupTouched", "CupId:" + other.gameObject.GetInstanceID().ToString());
        }
    }
}
