﻿using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Expressions
{
    public abstract class Expression:AstNode
    {
        public TypeDeclaration Type { get; set; }
    }
}
