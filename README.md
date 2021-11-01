<div  align=center>
    <img src="https://github.com/HIT-ReFreSH/IWannaGraduate/raw/master/logo.png" width = 30% height = 30%  />
</div>

# IWannaGraduate: 毕业学分计算工具

- 框架：[.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
- 适用于Windows，Linux或者Mac OS
- 基于MobileSuit的友好命令行应用
- MIT协议

## 使用方法

1. 从schemas文件夹中，选取合适的培养方案，替换程序根目录的`schema.yml`
2. 运行`HitRefresh.IWannaGraduate.exe`(windows)，或者`HitRefresh.IWannaGraduate.dll`(linux或Mac OS)
3. 输入指令

可用的指令包括：

|指令|功能|
|:---:|:---:|
|fill|根据培养方案，填写已修的内容|
|print|查看未修的课程或学分，以及已修的课程|
|load|从`dump.yml`加载培养方案和已修的内容|
|save|储存培养方案和已修的内容到`dump.yml`|

可用的培养方案包括：

|方案|状态|
|:---:|:---:|
|2018计算机类|未验证|
|2018计算机类英才|已验证|

## 注意事项

本工具仅帮助计算学分，不代表教务系统内部学分认证的真实情况。培养方案可能有误，作者不会对这些错误负责。

## 贡献代码和培养方案

请Fork后Pull Request。