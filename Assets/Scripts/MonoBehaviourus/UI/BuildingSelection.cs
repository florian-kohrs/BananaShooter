using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelection : MonoBehaviour
{

    public List<ScriptableObject> buildings;

    public int selectedBuildingIndex = -1;

    public ScriptableObject SelectedBuilding
    {
        get
        {
            if (selectedBuildingIndex < 0)
                return null;

            return buildings[selectedBuildingIndex];
        }
    }

    public void SelectBuilding(int index)
    {

    }


}
