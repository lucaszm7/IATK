using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Oculus.Interaction;

public class CubeButton : MonoBehaviour
{
    public AbstractVisualisation.PropertyType axis;
    public string dimensionName;

    public UnityEvent OnSelectEvent;

    public void Start()
    {
        var eventsHandler = GetComponent<MyEventWrapper>();
        OnSelectEvent.AddListener(PokeResponse);
        eventsHandler._whenSelect = OnSelectEvent;
    }

    public void PokeResponse()
    {
        var desk = FindObjectOfType<DeskFilters>();
        desk.UpdateAxis(axis, dimensionName);
    }
}
