using System;

namespace Training.Helpers
{
    public class NotFoundException : Exception
    {
        public string ParameterName { get; }
        public NotFoundException(string parameterName) : base(parameterName + " non trouvé")
        {
            ParameterName = parameterName;
        }
    }
}
