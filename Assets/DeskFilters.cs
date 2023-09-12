using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.Linq;

public class DeskFilters : MonoBehaviour
{
    [SerializeField]
    Visualisation visualisation;

    List<string> dimentions;

    [SerializeField]
    public GameObject buttonObject;
    
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

                instantiateButtonObject.transform.position = gameObject.transform.position + new Vector3(-0.01f, 0.01f, -0.01f) + (step * (j + 1));

                var cubeRenderer = instantiateButtonObject.GetComponent<Renderer>();
                Color customColor = new Color(0.1f, 0.2f, 0.3f, 1.0f) * j;
                cubeRenderer.material.SetColor("_Color", customColor);

                var buttonComponent = instantiateButtonObject.GetComponent<CubeButton>();
                buttonComponent.dimensionName = dimentions[j];
                buttonComponent.axis = axis;

                dimentionSelectables.Add(instantiateButtonObject);
            }
        }
    }

    public void UpdateAxis(AbstractVisualisation.PropertyType axis, string name)
    {
        var index = dimentions.FindIndex(x => x.Contains(name));

        if (visualisation != null)
        {
            if(axis == AbstractVisualisation.PropertyType.X)
                visualisation.xDimension = dimentions[index];

            if (axis == AbstractVisualisation.PropertyType.Y)
                visualisation.yDimension = dimentions[index];

            if (axis == AbstractVisualisation.PropertyType.Z)
                visualisation.zDimension = dimentions[index];

            visualisation.updateViewProperties(axis);
        }
    }

    private List<string> GetAttributesList()
    {
        List<string> dimensions = new List<string>();
        dimensions.Add("Undefined");

        if (visualisation != null)
        {
            for (int i = 0; i < (visualisation.dataSource?.DimensionCount ?? 0) - 1; ++i)
            {
                dimensions.Add(visualisation.dataSource[i].Identifier);
            }
        }

        return dimensions;
    }
}
