﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface IDisposeNotifable<T>
    {
        void TargetDisposed(T target);
    }   
   
}
