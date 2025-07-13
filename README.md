# Customized Broswer
这个浏览器可以让你在访问一些特定网址时，返回你已经设定好的网页。

# 方法
1.打开 `UrlHtmlMapping.cs` 文件； <br/>
2.找到 `UrlHtmlMapping` 类；<br/>
3.找到只读字段 `Mapping` 下方的大括号；<br/>
4.在其中添加 `{ new Uri(<网址>), @"<HTML网页内容>"},` ;<br/>

# 示例

``` CSharp
internal static class UrlHtmlMapping
{
  public static readonly Dictionary<Uri, string> Mapping = new Dictionary<Uri, string>
  {
    { new Uri("http://www.example.com"), @"<html><head><title>Example Page</title></head><body><h1>This is just an example page.</h1></body></html>" }, //在用户访问http://www.example.com时返回标题为 Example Page ，内容为 This is just an example page。
  };
}
```
