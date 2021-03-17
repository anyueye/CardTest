using UnityEngine;

public class ReadOnly : MonoBehaviour
{
    [NaughtyAttributes.ReadOnlyField]
    public int readOnlyInt = 5;
}
