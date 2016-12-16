# RTL8710AF_GCC
WiFi芯片RTL8710AF的GCC版SDK，采用C#脚本进行编译

# 目标
使用XScript支持的C#脚本编译RTL8710AF的SDK，自动部署必要的arm-none-eabi-gcc，不需要Cynwin等make环境。  
把SDK编译为静态库，供其它应用项目使用。

# 路线计划
1，拿到最新SDK v3.5a，安装最新arm-none-eabi-gcc和Cynwin，make编译通过  
2，简化makefile，去掉不必要的文件和编译选项  
3，参考make编译日志，编写C#版编译脚本，解除对Cynwin的依赖  
4，通过脚本实现模块化编译SDK，用户可简单选择自己需要的功能  

# GCC链接静态库顺序
GCC链接时对象文件和库文件的顺序需要非常小心。  
扫描分析对象文件时，同时完善导入导出表  
扫描分析静态库时，仅完善导入表配对以及必要导出，库里其它对象不做导出  
所以，对象文件任意更换顺序，也可以链接成功。而静态库不能为右边的静态库或对象文件提供导出配对。  

1，main.o + Lib/*.o + ram_1.r.o + Lib/*.a  
链接正确。分析静态库时第一个是lib_platform.a(startup.o)  
对象文件缺少的引用，右边的*.a会补上  
lib_platform.a(app_start.o) 需要链接 main 函数  
2，main.o + ram_1.r.o + lib_RTL8710.a + Lib/*.a  
链接少东西。分析静态库时第一个是lib_platform.a(hal_crypto.o)，然后开始分析lib_RTL8710.a  
main.o 缺少的引用，右边lib_RTL8710.a + Lib/*.a都会补上。  
但是lib_RTL8710.a与lib_platform.a相互依赖，导致大量符号无法链接。  
