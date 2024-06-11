﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions {
    public abstract class EntityException : Exception {

        protected EntityException(string message) : base(message) { }

    }
}
