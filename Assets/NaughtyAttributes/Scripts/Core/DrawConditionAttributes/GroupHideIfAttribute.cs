using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GroupHideIfAttribute : DrawGroupConditionAttribute
    {
        public GroupHideIfAttribute(params string[] groups)
        {
            this.groups = groups;
            this.canDraw = false;
        }

        public GroupHideIfAttribute(object compareObj, params string[] groups)
        : this(groups)
        {
            this.compareObj = compareObj;
        }
    }
}