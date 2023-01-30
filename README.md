# 自定义Logger学习Demo

## `ColorConsole`

这个是官方提供的例子, 可以在[这里](https://learn.microsoft.com/zh-cn/dotnet/core/extensions/custom-logging-provider)看

## `FileLogger`

是我参考了部分官方代码实现的简陋Demo,功能和实现都有限

### TODO

- 可自定义的输出文件配置
	- 可配置文件名
	- 可配置文件分割条件
		- 按日期时间分割
		- 按文件大小分割

- 不使用队列发送消息, 直接使用线程池处理, 或许会更好?
- 增加一个后台任务, 可以清理过久的日志
	- 可配置清理多少天之前的日志
- 可在线查看日志的UI ?


***

## 参考

### 文章

[官方文档](https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/logging)

[Microsoft.Extensions 探索 / 日志 Logger](https://zhuanlan.zhihu.com/p/465056786)


[ColorConsole微软官方例子](https://learn.microsoft.com/zh-cn/dotnet/core/extensions/custom-logging-provider)

### 项目

[TinyTxtLogger](https://github.com/wanglvhang/TinyTxtLogger)

[ZLogger](https://github.com/Cysharp/ZLogger)
