// 本脚本文件需要安装XScript才能双击运行，下载后解压缩运行一次XScript.exe即可完成安装
// https://github.com/NewLifeX/XScript/releases

using System.Collections.Generic;

var lib = "..\\Lib\\";

var build = Builder.Create("GCC") as GCC;
build.Init(false);
build.Cortex = 3;
build.Linux = true;
build.RebuildTime = 7 * 24 * 3600;
build.Specs = "nano.specs";
build.Entry = "Reset_Handler";
build.Defines.Add("CONFIG_PLATFORM_8195A");
build.Defines.Add("GCC_ARMCM3");
build.AddIncludes(lib, true, true);
build.AddLibs(lib, "*RTL8710*.a");
build.AddLibs(lib + "soc/realtek/8195a/misc/bsp/lib/common/GCC/", "lib_platform.a;lib_wlan.a;lib_p2p.a;lib_wps.a;lib_rtlstd.a;lib_websocket.a;lib_xmodem.a");
build.AddFiles(".", "*.c;*.cpp");
//build.AddFiles(lib, "*.c;*.cpp", true, "app_start.c");
// 引用这个文件可导致 lib_platform.a(startup.o) 被链接，从而确保整体成功
//build.AddFiles(lib, "low_level_io.c");
build.CompileAll();

var ram1 = lib + "soc/realtek/8195a/misc/bsp/image/ram_1.r.bin";
var cmd = "--rename-section .data=.loader.data,contents,alloc,load,readonly,data -I binary -O elf32-littlearm -B arm {0} obj/ram_1.r.o".F(ram1);
build.ObjCopy.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);
//build.Objs.Add("Obj\\ram_1.r.o");

build.ExtBuilds = "-lm -lc -lnosys -lgcc -nostartfiles";

// 打开链接详细信息，方便分析链接过程
//build.LinkVerbose = true;
var rs = build.Build(".axf");
if(rs != 0) return;

/*build.Debug = true;
build.CompileAll();
build.Build();*/

var axf = "Obj/Sample.axf";

// 输出排序的nmap文件，记录地址名称对照
var nm = build.ToolPath.CombinePath("bin/arm-none-eabi-nm.exe");
cmd = "{0}".F(axf);
var list = new List<String>();
nm.Run(cmd, 3000, msg => list.Add(msg));

// 导出
cmd = "-j .image2.start.table -j .ram_image2.text -j .ram_image2.rodata -j .ram.data -Obinary {0} ram_2.bin".F(axf);
build.ObjCopy.Run(cmd, 3000, NewLife.Log.XTrace.WriteLine);

var start = list.FirstOrDefault(e=>e.Trim().EndsWith("__ram_image2_text_start__")).Substring(null, " ").ToHex().ToUInt32(0, false);
//var end   = list.FirstOrDefault(e=>e.Trim().EndsWith("__ram_image2_text_end__")).Substring(null, " ").ToHex().ToUInt32(0, false);
//Console.WriteLine("Start: 0x{0:X8} End:0x{1:X8}", start, end);

// 构建头部。4大小+4地址+8签名
var img2 = File.ReadAllBytes("ram_2.bin");
var buf = new Byte[8];
buf.Write((UInt32)img2.Length);
buf.Write(start, 4);

var fs = File.Create("ram_all.bin");
// 写入img1并填充44k
fs.Write(File.OpenRead(lib + "soc/realtek/8195a/misc/bsp/image/ram_1.p.bin"));
var pad = new Byte[44 * 1024 - (Int32)fs.Length];
Buffer.SetByte(pad, 0, 0xFF);
fs.Write(pad);
// img2开始
fs.Write(buf);
fs.Write("81958711".GetBytes());
fs.Write(img2);
fs.Close();

fs = File.Create("ota.bin");
fs.Write(buf);
fs.Write("FFFFFFFF".ToHex());
fs.Write(img2);
fs.Close();
