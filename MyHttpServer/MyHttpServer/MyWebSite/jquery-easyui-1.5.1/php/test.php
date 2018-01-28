一般而言，通过隐藏的手段提高安全性被认为是作用不大的做法。但某些情况下，尽可能的多增加一份安全性都是值得的。 

一些简单的方法可以帮助隐藏 PHP，这样做可以提高攻击者发现系统弱点的难度。在 php.ini 文件里设置 expose_php = off ，可以减少他们能获得的有用信息。 

 

另一个策略就是让类似于 Apache 的 Web 服务端调用 PHP 解释其它扩展名的 PHP 文件。无论是通过 .htaccess 文件还是 Apache 的配置文件，都可以设置能误导攻击者的文件扩展名： 例子 32-1. 把 PHP 隐藏类另一种语言

# 使PHP看上去像其它的编程语言 
AddType application/x-httpd-php .asp .py .pl
 
 
或者干脆彻底隐藏它： 例子 32-2. 使用不知明的扩展名作为PHP的扩展名

# 使PHP看上去像不知名的文件类型 
AddType application/x-httpd-php .bop .foo .133t
 
 
或者把它隐藏为 HTML 页面，这样 PHP 会解释所有的 HTML 页面，因此这样做会为服务器增加一些负担： 例子 32-3. 把 HTML 文件的扩展名作为 PHP 的扩展名

# 使PHP看上去像HTML页面 
AddType application/x-httpd-php .htm .html
 
 
完成以上工作后，必须把 PHP 文件的扩展名改为以上的扩展名。这种通过隐藏来提高安全性的手段，虽然防御能力不强，但操作起来较为简单。 