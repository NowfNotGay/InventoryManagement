using Core.ExampleClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Example
{
    public interface IMessageContentProvider
    {
        Task<IEnumerable<MessageContent>> GetByTitle(string title);
    }
}
