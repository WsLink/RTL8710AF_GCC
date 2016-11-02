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
