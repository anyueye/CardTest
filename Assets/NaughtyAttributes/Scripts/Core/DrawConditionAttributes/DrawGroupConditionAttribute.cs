using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NaughtyAttributes
{
    public abstract class DrawGroupConditionAttribute : NaughtyAttribute
    {
        public string[] groups { get; protected set; }
        public bool canDraw { get; protected set; }
        public object compareObj { get; protected set; }
    }
}