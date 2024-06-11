using System.Reflection;

namespace ExchangeGood.API {
    public static class AssemblyReference {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
