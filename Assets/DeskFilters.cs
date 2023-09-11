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
    int xAxis;
    int yAxis;
    int zAxis;

    List<GameObject> dimentionSelectables;

    // Start is called before the first frame update
    public void Start()
    {
        dimentions = GetAttributesList();
        dimentionSelectables = new List<GameObject>();

        for (var i = 0; i < dimentions.Count; ++i)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(cube.GetComponent<Rigidbody>());
            cube.transform.localScale = new Vector3( .02f, .02f, .02f);
            var step = new Vector3(0.05f, 0, 0);
            cube.transform.position = gameObject.transform.position + new Vector3(0.0f, 0.0f, -0.1f) + (step * i);

            var cubeRenderer = cube.GetComponent<Renderer>();
            Color customColor = new Color(0.1f, 0.2f, 0.3f, 1.0f) * i;
            cubeRenderer.material.SetColor("_Color", customColor);

            dimentionSelectables.Add(cube);
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }

    public void UpdateAxis(string name)
    {
        xAxis = 1;
        yAxis = 2;
        zAxis = 3;

        if (visualisation != null)
        {
            visualisation.xDimension = dimentions[xAxis];
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.X);

            visualisation.yDimension = dimentions[yAxis];
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Y);

            visualisation.zDimension = dimentions[zAxis];
            visualisation.updateViewProperties(AbstractVisualisation.PropertyType.Z);
        }

        // visualisation.updateProperties();
        // visualisation.CreateVisualisation(AbstractVisualisation.VisualisationTypes.SCATTERPLOT);
    }

    private List<string> GetAttributesList()
    {
        List<string> dimensions = new List<string>();
        dimensions.Add("Undefined");
        for (int i = 0; i < visualisation.dataSource.DimensionCount; ++i)
        {
            dimensions.Add(visualisation.dataSource[i].Identifier);
        }

        xAxis = 4;
        yAxis = 5;
        zAxis = 6;

        return dimensions;
    }
}
