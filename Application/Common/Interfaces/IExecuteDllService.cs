using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IExecuteDllService
    {
        Task<object> ProcessExecute(string dll_path, string dll_name, string method_name, object[] param);
    }
}
