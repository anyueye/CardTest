﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GroupShowIfAttribute : DrawGroupConditionAttribute
    {
        public GroupShowIfAttribute(params string[] groups)
        {
            this.groups = groups;
            this.canDraw = true;
        }
        public GroupShowIfAttribute(object compareObj, params string[] groups)
        : this(groups)
        {
            this.compareObj = compareObj;
        }
    }
}