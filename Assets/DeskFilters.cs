using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.Linq;
using TMPro;
using System;

public class DeskFilters : MonoBehaviour
{
    [SerializeField]
    Visualisation visualisation;

    [SerializeField]
    ScatterplotVisualisation scatterVisualisation;

    List<string> dimentions;

    [SerializeField]
    public GameObject buttonObject;

    private int xDimension;
    private int yDimension;
    private int zDimension;
    private int ColorDimension;

    private Color selectedColor = Color.blue;
    private Color unselectedColor = Color.black;

    List<GameObject> dimentionSelectables;

    // Start is called before the first frame update
    public void Start()
    {
        dimentions = GetAttributesList();
        dimentionSelectables = new List<GameObject>();

        ColorDimension = 3;
        xDimension = 4;
        yDimension = 5;
        zDimension = 6;

        Func<AbstractVisualisation.PropertyType, int, Vector3, bool, bool> createDimension = (dimension, indexSelected, step, deskDimension) =>
        {
            for (var j = 0; j < dimentions.Count; ++j)
            {
                var instantiateButtonObject = Instantiate(this.buttonObject, gameObject.transform);

                var graphPos = gameObject.transform.position + new Vector3(-0.02f, 0.02f, -0.02f) + (step * (j + 1));
                var deskPos = new Vector3(-0.168799996f, 0.75029999f, 0.300900012f) + (step * (j + 1));
                instantiateButtonObject.transform.position = deskDimension ? deskPos : graphPos;
                instantiateButtonObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                var cubeText = instantiateButtonObject.GetComponentInChildren<TextMeshPro>();
                if (cubeText != null)
                    cubeText.text = dimentions[j];

                var cubeIndexRenderer = instantiateButtonObject.GetComponent<MeshRenderer>();
                cubeIndexRenderer.material.SetColor("_Color", j == indexSelected ? selectedColor : unselectedColor);

                var buttonComponent = instantiateButtonObject.GetComponent<CubeButton>();
                buttonComponent.dimensionName = dimentions[j];
                buttonComponent.axis = dimension;

                dimentionSelectables.Add(instantiateButtonObject);
            }
            return true;
        };

        createDimension(AbstractVisualisation.PropertyType.X, xDimension, new Vector3(0.05f, 0, 0), false);
        createDimension(AbstractVisualisation.PropertyType.Y, yDimension, new Vector3(0, 0.05f, 0), false);
        createDimension(AbstractVisualisation.PropertyType.Z, zDimension, new Vector3(0, 0, 0.05f), false);
        createDimension(AbstractVisualisation.PropertyType.Colour, ColorDimension, new Vector3(0.05f, 0, 0), true);

        visualisation.updateView();
    }

    public void UpdateAxis(AbstractVisualisation.PropertyType axis, string name)
    {
        int unselectedDimensionNameIndex = 0;
        if (axis == AbstractVisualisation.PropertyType.X)
            unselectedDimensionNameIndex = xDimension;
        else if (axis == AbstractVisualisation.PropertyType.Y)
            unselectedDimensionNameIndex = yDimension;
        else if (axis == AbstractVisualisation.PropertyType.Z)
            unselectedDimensionNameIndex = zDimension;
        else if (axis == AbstractVisualisation.PropertyType.Colour)
            unselectedDimensionNameIndex = ColorDimension;

        var selectedCubeIndex = dimentionSelectables.FindIndex(i => i.GetComponent<CubeButton>().dimensionName == name && i.GetComponent<CubeButton>().axis == axis);
        var unselectedCubeIndex = dimentionSelectables.FindIndex(i => i.GetComponent<CubeButton>().dimensionName == dimentions[unselectedDimensionNameIndex] && i.GetComponent<CubeButton>().axis == axis);

        if (unselectedCubeIndex == selectedCubeIndex)
            return;

        var cubeIndexRenderer = dimentionSelectables[selectedCubeIndex].GetComponent<MeshRenderer>();
        cubeIndexRenderer.material.SetColor("_Color", selectedColor);

        var lastestCubeIndexRenderer = dimentionSelectables[unselectedCubeIndex].GetComponent<MeshRenderer>();
        lastestCubeIndexRenderer.material.SetColor("_Color", unselectedColor);

        var selectedDimensionNameIndex = dimentions.FindIndex(x => x.Contains(name));

        if (visualisation != null)
        {
            if (axis == AbstractVisualisation.PropertyType.X)
                xDimension = selectedDimensionNameIndex;
            if (axis == AbstractVisualisation.PropertyType.Y)
                yDimension = selectedDimensionNameIndex;
            if (axis == AbstractVisualisation.PropertyType.Z)
                zDimension = selectedDimensionNameIndex;
            if (axis == AbstractVisualisation.PropertyType.Colour)
                ColorDimension = selectedDimensionNameIndex;

            visualisation.xDimension = dimentions[xDimension];
            visualisation.yDimension = dimentions[yDimension];
            visualisation.zDimension = dimentions[zDimension];
            visualisation.colourDimension = dimentions[ColorDimension];

            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.X);
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Y);
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Z);
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Colour);
        }
    }

    private List<string> GetAttributesList()
    {
        List<string> dimensions = new List<string>();

        if (visualisation != null)
        {
            for (int i = 0; i < (visualisation.dataSource?.DimensionCount ?? 0); ++i)
            {
                dimensions.Add(visualisation.dataSource[i].Identifier);
            }
        }

        return dimensions;
    }
}
