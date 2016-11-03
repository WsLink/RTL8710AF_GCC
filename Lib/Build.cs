// 本脚本文件需要安装XScript才能双击运行，下载后解压缩运行一次XScript.exe即可完成安装
// https://github.com/NewLifeX/XScript/releases

var build = Builder.Create("GCC");
build.Init(false);
build.Cortex = 3;
build.Linux = true;
build.Partial = true;	// 分部编译链接
build.Defines.Add("CONFIG_PLATFORM_8195A");
build.Defines.Add("GCC_ARMCM3");
build.AddIncludes(".".FindUp("Lib") + "\\..\\", true, true);
build.AddFiles(".", "*.c;*.cpp;*.s", true, "rtl8195a_pcm.c;rtl8195a_sdio_device.c;tcpecho.c;udpecho.c");
build.CompileAll();
build.BuildLib(".a");
