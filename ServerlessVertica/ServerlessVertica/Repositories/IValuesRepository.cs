using System.Collections.Generic;

namespace ServerlessVertica.Repositories
{
    public interface IValuesRepository
    {
        IEnumerable<string> GetValues();
    }
}
