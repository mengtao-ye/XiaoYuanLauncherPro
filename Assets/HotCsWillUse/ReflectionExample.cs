using System.Reflection;
using UnityEngine;

public class ReflectionExample : MonoBehaviour
{
    public int myInt = 10;

    private void Start()
    {
        PropertyInfo propertyInfo = typeof(ReflectionExample).GetProperty("myInt");
        Debug.Log("myInt = " + propertyInfo.GetValue(this));
    }
}