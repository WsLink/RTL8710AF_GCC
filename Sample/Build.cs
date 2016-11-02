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
        static void Main(String[] args)
        {
            var build = Builder.Create("GCC");
            build.Init(false);
            build.Cortex = 3;
			build.Linux = true;
			build.Defines.Add("CONFIG_PLATFORM_8195A");
			build.AddIncludes("..\\GCCLib", true, true);
			build.Libs.Clear();
			build.AddLibs("..\\GCCLib", "RTL871x*.lib");
			build.AddLibs("..\\GCCLib\\misc\\bsp\\lib", "*.a");
			//build.AddLibs("..\\SmartOS", "*.a");
            build.AddFiles(".", "*.c;*.cpp");
            build.CompileAll();
			
			var ram1 = "../GCCLib/soc/realtek/8195a/misc/bsp/image/ram_1.r.bin";
			var cmd = "--rename-section .data=.loader.data,contents,alloc,load,readonly,data -I binary -O elf32-littlearm -B arm {0} obj/ram_1.r.o".F(ram1);
			build.ObjCopy.Run(cmd, 3000, XTrace.WriteLine);
			
			build.ExtBuilds.Add("-L../GCCLib/soc/realtek/8195a/misc/bsp/lib/common/GCC/ -l_platform -l_wlan_mp -l_p2p -l_wps -l_rtlstd -l_websocket -l_xmodem -lm -lc -lnosys -lgcc");
			
            build.Build();

            build.Debug = true;
            build.CompileAll();
            build.Build();
		}
    }
}
