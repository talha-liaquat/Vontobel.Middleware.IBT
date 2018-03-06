﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Core
{
    public interface ITransformation<T> : ITransformation
    {
        T Transform(T source, Dictionary<string, string> arguments);
    }
}