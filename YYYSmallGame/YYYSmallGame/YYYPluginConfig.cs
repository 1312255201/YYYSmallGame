using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YYYSmallGame
{
    public sealed class YYYPluginConfig : IConfig
    {
        [Description("是否开启插件")]
        public bool IsEnabled { get; set; } = true;
        [Description("是否开启Debug")]
        public bool debugmod { get; set; } = true;
    }
}
