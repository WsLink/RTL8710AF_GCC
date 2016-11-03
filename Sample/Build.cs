// 本脚本文件需要安装XScript才能双击运行，下载后解压缩运行一次XScript.exe即可完成安装
// https://github.com/NewLifeX/XScript/releases

using System.Collections.Generic;

var build = Builder.Create("GCC") as GCC;
build.Init(false);
build.Cortex = 3;
build.Linux = true;
build.Specs = "nano.specs";
build.Entry = "Reset_Handler";
build.Defines.Add("CONFIG_PLATFORM_8195A");
build.Defines.Add("GCC_ARMCM3");
build.AddIncludes("..\\Lib", true, true);
build.AddLibs("..\\Lib", "*RTL8710*.a");
build.AddLibs("../Lib/soc/realtek/8195a/misc/bsp/lib/common/GCC/", "*.a");
build.AddFiles(".", "*.c;*.cpp");
build.CompileAll();

var ram1 = "../Lib/soc/realtek/8195a/misc/bsp/image/ram_1.r.bin";
var cmd = "--rename-section .data=.loader.data,contents,alloc,load,readonly,data -I binary -O elf32-littlearm -B arm {0} obj/ram_1.r.o".F(ram1);
build.ObjCopy.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);

/*build.ExtBuilds.Add("-L../Lib/soc/realtek/8195a/misc/bsp/lib/common/GCC/ -l_platform -l_wlan_mp -l_p2p -l_wps -l_rtlstd -l_websocket -l_xmodem -lm -lc -lnosys -lgcc");*/
build.ExtBuilds.Add("-lm -lc -lnosys -lgcc");

build.Build(".hex");

/*build.Debug = true;
build.CompileAll();
build.Build();*/

var axf = "Obj/Sample.axf";

// 输出排序的nmap文件，记录地址名称对照
var nm = build.ToolPath.CombinePath("bin/arm-none-eabi-nm.exe");
//cmd = "Obj/Sample.axf | sort > Obj/Sample.nmap".F();
cmd = "{0}".F(axf);
var list = new List<String>();
nm.Run(cmd, 3000, msg => list.Add(msg));
list.Sort();
var nmap = "Obj/Sample.nmap";
File.WriteAllLines(nmap, list.ToArray());

// 导出
cmd = "-j .image2.start.table -j .ram_image2.text -j .ram_image2.rodata -j .ram.data -Obinary {0} ram_2.bin".F(axf);

var pick = "../Tools/pick.exe";
var padding = "../Tools/padding.exe";
var checksum = "../Tools/checksum.exe";

cmd = "0x`grep __ram_image2_text_start__ {0} | gawk '{{print $1}}'` 0x`grep __ram_image2_text_end__ {0} | gawk '{{print $1}}'` ram_2.bin ram_2.p.bin body+reset_offset+sig".F(nmap);
pick.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);

cmd = cmd.Replace("ram_2.p", "ram_2.ns").TrimEnd("+sig");
pick.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);

File.Copy("../Lib/soc/realtek/8195a/misc/bsp/image/ram_1.p.bin", "ram_1.p.bin", true);
cmd = "44k 0xFF ram_1.p.bin";
padding.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);
