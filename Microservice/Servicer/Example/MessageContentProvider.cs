using Base.BaseService;
using Base.Example;
using Core.ExampleClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicer.Example
{
    public class MessageContentProvider : IMessageContentProvider, ICRUD_Service<MessageContent, int>
    {
        public Task<MessageContent> Create(MessageContent entity)
        {
            throw new NotImplementedException();
        }

        public Task<MessageContent> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MessageContent> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageContent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageContent>> GetByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public Task<MessageContent> Update(MessageContent entity)
        {
            throw new NotImplementedException();
        }

        Task<string> ICRUD_Service<MessageContent, int>.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
