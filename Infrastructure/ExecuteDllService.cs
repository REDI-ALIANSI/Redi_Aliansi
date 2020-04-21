using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ExecuteDllService : IExecuteDllService
    {
        public Task<object> ProcessExecute(string dll_path,string dll_name, string method_name, object[] param)
        {
            object result = null;
            try
            {
                Assembly assembly = Assembly.LoadFile(@dll_path);
                //objLogs.LogStats("DLL PATH:" + dll_path,q);
                Type type = assembly.GetType("SMS_DLL." + dll_name);
                if (type != null)
                {
                    MethodInfo methodInfo = type.GetMethod(method_name);
                    if (methodInfo != null)
                    {
                        //object result = null;
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        object classInstance = Activator.CreateInstance(type, null);
                        if (parameters.Length == 0) // if method dont have parameters required
                        {
                            //result = methodInfo.Invoke(classInstance, null);
                            result = null;
                        }
                        else // if method requires parameter
                        {
                            //object[] parametersArray = new object[] { param };
                            result = methodInfo.Invoke(classInstance, param);
                        }
                    }
                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
