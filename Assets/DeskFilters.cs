using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.Linq;
using TMPro;
using System;
using Zinnia.Association;

public class DeskFilters : MonoBehaviour
{
    [SerializeField]
    private Visualisation visualisation;

    private List<string> dimentions;
    private List<string> dimensionsUsed;

    [SerializeField]
    private GameObject buttonObject;

    private GameObject Desk;

    private int xDimension;
    private int yDimension;
    private int zDimension;
    private int ColorDimension;
    private int SizeDimension;

    private Color selectedColor = Color.blue;
    private Color unselectedColor = Color.black;

    private List<GameObject> dimentionSelectables;

    private AbstractVisualisation.PropertyType dimensionSelected;

    // Start is called before the first frame update
    public void Start()
    {
        dimentions = GetAttributesList();
        dimensionsUsed = new List<string>() { "X", "Y", "Z", "Color", "Size"};
        dimentionSelectables = new List<GameObject>();

        // find object with tag "Desk"
        Desk = GameObject.FindGameObjectsWithTag("Desk").FirstOrDefault();

        dimensionSelected = AbstractVisualisation.PropertyType.X;
        ColorDimension = 3;
        xDimension = 4;
        yDimension = 5;
        zDimension = 6;
        SizeDimension = 7;

        Func<List<string>, AbstractVisualisation.PropertyType, int, Vector3, Vector3, bool> createDimension = (options, dimension, indexSelected, step, offset) =>
        {
            for (var j = 0; j < options.Count; ++j)
            {
                var instantiateButtonObject = Instantiate(this.buttonObject, gameObject.transform);

                var deskPos = offset + (step * (j + 1));

                var localScale = new Vector3(0.03f, 0.03f, 0.03f);

                instantiateButtonObject.transform.position = deskPos;
                instantiateButtonObject.transform.parent = Desk.transform;
                instantiateButtonObject.transform.localScale = localScale;

                var cubeText = instantiateButtonObject.GetComponentInChildren<TextMeshPro>();
                if (cubeText != null)
                    cubeText.text = options[j];

                var cubeIndexRenderer = instantiateButtonObject.GetComponent<MeshRenderer>();
                cubeIndexRenderer.material.SetColor("_Color", j == indexSelected ? selectedColor : unselectedColor);

                var buttonComponent = instantiateButtonObject.GetComponent<CubeButton>();
                buttonComponent.dimensionName = options[j];
                buttonComponent.axis = dimension;

                dimentionSelectables.Add(instantiateButtonObject);
            }
            return true;
        };

        var step = new Vector3(0.07f, 0, 0);
        createDimension(dimentions,     AbstractVisualisation.PropertyType.OriginDimension, xDimension, step, new Vector3(-0.60f, 0.815f, 0.40f));
        createDimension(dimensionsUsed, AbstractVisualisation.PropertyType.DimensionChange, 0, step, new Vector3(-0.3f, 0.815f, 0.30f));

        visualisation.updateView();
    }

    public void UpdateAxis(AbstractVisualisation.PropertyType axis, string name)
    {
        string dimensionName = name;
        if(axis == AbstractVisualisation.PropertyType.DimensionChange)
        {
            dimensionSelected = ToDimension(name);
            if (dimensionSelected == AbstractVisualisation.PropertyType.X)
                name = dimentions[xDimension];
            if (dimensionSelected == AbstractVisualisation.PropertyType.Y)
                name = dimentions[yDimension];
            if (dimensionSelected == AbstractVisualisation.PropertyType.Z)
                name = dimentions[zDimension];
            if (dimensionSelected == AbstractVisualisation.PropertyType.Colour)
                name = dimentions[ColorDimension];
            if (dimensionSelected == AbstractVisualisation.PropertyType.Size)
                name = dimentions[SizeDimension];
        }
        else
        {
            dimensionName = ToString(dimensionSelected);
        }

        TurnAllCubesUnselectedColor();
        TurnCubeSelected(dimensionName);
        TurnCubeSelected(name);

        var selectedDimensionNameIndex = dimentions.FindIndex(x => x.Contains(name));

        if (visualisation != null)
        {
            if (dimensionSelected == AbstractVisualisation.PropertyType.X)
                xDimension = selectedDimensionNameIndex;
            if (dimensionSelected == AbstractVisualisation.PropertyType.Y)
                yDimension = selectedDimensionNameIndex;
            if (dimensionSelected == AbstractVisualisation.PropertyType.Z)
                zDimension = selectedDimensionNameIndex;
            if (dimensionSelected == AbstractVisualisation.PropertyType.Colour)
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

    private AbstractVisualisation.PropertyType ToDimension(string name)
    {
        switch(name)
        {
            case "X":
                return AbstractVisualisation.PropertyType.X;
            case "Y":
                return AbstractVisualisation.PropertyType.Y;
            case "Z":
                return AbstractVisualisation.PropertyType.Z;
            case "Color":
                return AbstractVisualisation.PropertyType.Colour;
            case "Size":
                return AbstractVisualisation.PropertyType.Size;
            default:
                return AbstractVisualisation.PropertyType.X;
        }
    }

    private string ToString(AbstractVisualisation.PropertyType type)
    {
        switch (type)
        {
            case AbstractVisualisation.PropertyType.X:
                return "X";
            case AbstractVisualisation.PropertyType.Y:
                return "Y";
            case AbstractVisualisation.PropertyType.Z:
                return "Z";
            case AbstractVisualisation.PropertyType.Colour:
                return "Color";
            case AbstractVisualisation.PropertyType.Size:
                return "Size";
            default:
                return "ERROR";
        }
    }

    private void TurnAllCubesUnselectedColor()
    {
        foreach (var cube in dimentionSelectables)
        {
            var cubeIndexRenderer = cube.GetComponent<MeshRenderer>();
            cubeIndexRenderer.material.SetColor("_Color", unselectedColor);
        }
    }

    private void TurnCubeSelected(string name)
    {
        var selectedCubeIndex = dimentionSelectables.FindIndex(i => i.GetComponent<CubeButton>().dimensionName == name);
        var cubeIndexRenderer = dimentionSelectables[selectedCubeIndex].GetComponent<MeshRenderer>();
        cubeIndexRenderer.material.SetColor("_Color", selectedColor);
    }

}
