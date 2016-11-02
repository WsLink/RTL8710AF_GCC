// 本脚本文件需要安装XScript才能双击运行，下载后解压缩运行一次XScript.exe即可完成安装
// https://github.com/NewLifeX/XScript/releases

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Win32;
using NewLife.Log;

namespace NewLife.Reflection
{
    public class ScriptEngine
    {
        static void Main()
        {
            var build = Builder.Create("GCC");
            build.Init(false);
			build.Cortex = 3;
			build.Defines.Add("M3");
			build.Defines.Add("CONFIG_PLATFORM_8195A");
			build.Defines.Add("GCC_ARMCM3");
			build.Defines.Add("ARDUINO_SDK");
			build.Defines.Add("F_CPU=166000000L");
			build.Defines.Add("LWIP_TIMEVAL_PRIVATE=0");
			build.AddIncludes(".", true, true);
			build.AddFiles("common", "*.c;*.cpp;*.s", true, "tcpecho.c;udpecho.c");
			build.AddFiles("os", "*.c;*.cpp;*.s", true, "");
			build.AddFiles("soc\\realtek\\8195a", "*.c;*.cpp;*.s", true, "rtl8195a_pcm.c;rtl8195a_sdio_device.c;app_start.c;cmsis_nvic.c");
            build.CompileAll();
            build.BuildLib();

			build.Debug = true;
            build.CompileAll();
            build.BuildLib();
        }
    }

}
