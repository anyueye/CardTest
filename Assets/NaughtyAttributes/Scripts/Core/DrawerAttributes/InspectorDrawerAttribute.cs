using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InspectorDrawerAttribute : DrawerAttribute
    {
    }
}