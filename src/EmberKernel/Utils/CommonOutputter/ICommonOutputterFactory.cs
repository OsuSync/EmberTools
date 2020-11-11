using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Utils.CommonOutputter
{
    public interface ICommonOutputterFactory : IComponent
    {
        IOutputter CreateMemoryMappingFileOutputter(string name);
        IOutputter CreateDiskFileOutputter(string path);
        //IOutputter CreateWebsocketOutputter(string wsPath);

        /// <summary>
        /// 根据提供的参数内容，自动提供对应合适的输出对象
        /// </summary>
        /// <param name="name">比如提供"mmf://xxx"或者"x://xxx.xxx"或者:"ws://xxx"</param>
        /// <returns></returns>
        IOutputter CreateOutputterByDefinition(string name);
    }
}
