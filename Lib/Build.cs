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
			//build.ToolPath = @"E:\Auto\RTL871x\GCC\tools\arm-none-eabi-gcc\4.8.3-2014q1";
            build.Init(false);
			build.Cortex = 3;
			build.Linux = true;
			build.Includes.Clear();
			build.Defines.Add("M3");
			build.Defines.Add("CONFIG_PLATFORM_8195A");
			build.Defines.Add("GCC_ARMCM3");
			build.Defines.Add("ARDUINO_SDK");
			build.Defines.Add("F_CPU=166000000L");
			build.AddIncludes("..\\", true, true);
			build.AddFiles(".", "*.c;*.cpp;*.s", true, "rtl8195a_pcm.c;rtl8195a_sdio_device.c;tcpecho.c;udpecho.c");
            build.CompileAll();
            build.BuildLib();
        }
    }
}
