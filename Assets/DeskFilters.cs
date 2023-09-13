using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.Linq;
using TMPro;

public class DeskFilters : MonoBehaviour
{
    [SerializeField]
    Visualisation visualisation;

    [SerializeField]
    ScatterplotVisualisation scatterVisualisation;

    List<string> dimentions;

    [SerializeField]
    public GameObject buttonObject;

    private int xAxis;
    private int yAxis;
    private int zAxis;

    List<GameObject> dimentionSelectables;

    // Start is called before the first frame update
    public void Start()
    {
        dimentions = GetAttributesList();
        dimentionSelectables = new List<GameObject>();

        Vector3 step = Vector3.zero;
        AbstractVisualisation.PropertyType axis = AbstractVisualisation.PropertyType.X;

        for (var i = 0; i < 3; ++i)
        {
            if (i == 0)
            {
                axis = AbstractVisualisation.PropertyType.X;
                step = new Vector3(0.05f, 0, 0);
            }

            else if(i == 1)
            {
                axis = AbstractVisualisation.PropertyType.Y;
                step = new Vector3(0.0f, 0.05f, 0.0f);
            }

            else if (i == 2)
            {
                axis = AbstractVisualisation.PropertyType.Z;
                step = new Vector3(0.0f, 0.0f, 0.05f);
            }

            for (var j = 0; j < dimentions.Count; ++j)
            {
                var instantiateButtonObject = Instantiate(this.buttonObject, gameObject.transform);

                instantiateButtonObject.transform.position = gameObject.transform.position + new Vector3(-0.02f, 0.02f, -0.02f) + (step * (j + 1));

                //var cubeRenderer = instantiateButtonObject.GetComponent<MeshRenderer>();
                //Color customColor = new Color(0.1f, 0.2f, 0.3f, 1.0f) * j;
                //cubeRenderer.material.SetColor("_Color", customColor);

                instantiateButtonObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                var buttonComponent = instantiateButtonObject.GetComponent<CubeButton>();
                buttonComponent.dimensionName = dimentions[j];
                buttonComponent.axis = axis;

                var buttonText = instantiateButtonObject.GetComponentInChildren<TextMeshPro>();

                dimentionSelectables.Add(instantiateButtonObject);
            }
        }

        xAxis = 4;
        yAxis = 5;
        zAxis = 6;

        visualisation.updateView();
    }

    public void UpdateAxis(AbstractVisualisation.PropertyType axis, string name)
    {
        var cubeIndex = dimentionSelectables.FindIndex(i => i.GetComponent<CubeButton>().dimensionName == name && i.GetComponent<CubeButton>().axis == axis);
        var cubeIndexRenderer = dimentionSelectables[cubeIndex].GetComponent<MeshRenderer>();
        cubeIndexRenderer.material.SetColor("_Color", new Color( 0.0f, 0.0f, 1.0f));

        int axisLatestCubeSelected;
        if (axis == AbstractVisualisation.PropertyType.X)
            axisLatestCubeSelected = xAxis;
        else if (axis == AbstractVisualisation.PropertyType.Y)
            axisLatestCubeSelected = yAxis;
        else
            axisLatestCubeSelected = zAxis;

        var lastestCubeIndexRenderer = dimentionSelectables[axisLatestCubeSelected].GetComponent<MeshRenderer>();
        lastestCubeIndexRenderer.material.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));

        var index = dimentions.FindIndex(x => x.Contains(name));

        if (visualisation != null)
        {
            if (axis == AbstractVisualisation.PropertyType.X)
                xAxis = index;
            if (axis == AbstractVisualisation.PropertyType.Y)
                yAxis = index;
            if (axis == AbstractVisualisation.PropertyType.Z)
                zAxis = index;

            visualisation.xDimension = dimentions[xAxis];
            visualisation.yDimension = dimentions[yAxis];
            visualisation.zDimension = dimentions[zAxis];

            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.X);
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Y);
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Z);
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
