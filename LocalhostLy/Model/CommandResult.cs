using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePlatform.Common.Execution;

namespace ThePlatform.Core.Data
{
    public class CommandResult<T>
    {
        public static implicit operator CommandResult<object>(CommandResult<T> a_This)
        {
            return new CommandResult<object>(a_This.Object, a_This.Result);
        }

        public CommandResult()
        {

        }

        public CommandResult(T a_Object, OperationResult a_Result)
        {
            Object = a_Object;
            Result = a_Result;
        }

        public T Object { get; set; }
        public OperationResult Result { get; set; }
    }
}
