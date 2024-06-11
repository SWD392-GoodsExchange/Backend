
namespace ExchangeGood.Repository.Exceptions {
    public sealed class ValidationException : EntityException {

        public ValidationException(IDictionary<string, string[]> errors) : base("One or more validation errors occured!")
            => Errors = errors;

        public IDictionary<string, string[]> Errors { get; }
    }
}
