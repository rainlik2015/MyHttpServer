<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<title>隐藏PHP文件扩展名</title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
</head>
<script src="js/check.js"></script>
<body>
<table width="1014" height="706" border="0" align="center" cellpadding="0" cellspacing="0" background="images/form1.jpg">
  <tr>
    <td width="133" height="228">&nbsp;</td>
    <td width="750">&nbsp;</td>
    <td width="113">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="center" valign="top">
	<form name="form_reg" method="post" action="index_ok.shtml" onSubmit="return chkreginfo(form_reg,'all')">
      <table width="620" height="262" border="0" align="center" cellpadding="0" cellspacing="0">
			<?php 
			$m=1;
			$n='action';
			?>
                <tr>
                  <td width="120" height="30"><div align="right">用户名：</div></td>
                  <td colspan="2">&nbsp;<input type="text" name="recuser" size="20" class="inputcss" onBlur="chkreginfo(form_reg,0)">
                    <font color="#FF0000">*</font>&nbsp;
                    <a href="test-<?php echo $m;?>-<?php echo $n;?>.shtml">补充内容</a>
                  <div id="chknew_recuser" style="color:#FF0000"></div></td>
                </tr>
				
                <tr>
                  <td height="30"><div align="right">详细联系地址：</div></td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="address" size="60" class="inputcss" onBlur="chkreginfo(form_reg,1)">
                    <font color="#FF0000">*</font>&nbsp;
                  <div id="chknew_address" style="color:#FF0000"></div></td>
                </tr>
                <tr>
                  <td height="30"><div align="right">邮政编码：</div></td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="postalcode" size="20" class="inputcss" onBlur="chkreginfo(form_reg,2)"><div id="chknew_postalcode" style="color:#FF0000"></div></td>
                </tr>
				<tr>
                  <td height="30"><div align="right">QQ号码：</div></td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="qq" size="20" class="inputcss" onBlur="chkreginfo(form_reg,3)">
                    <font color="#FF0000">*</font>&nbsp;
                  <div id="chknew_qq" style="color:#FF0000"></div></td>
                </tr>
				<tr>
                  <td height="30"><div align="right">E-mail：</div></td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="email" size="20" class="inputcss" onBlur="chkreginfo(form_reg,4)">
                    <font color="#FF0000">*</font>&nbsp;<font color="#999999">请务必正确填写您的邮箱</font>
                    <div id="chknew_email" style="color:#FF0000"></div></td>
                </tr>
                <tr>
                  <td height="30" align="right">固定电话：</td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="gtel" size="20" class="inputcss" onBlur="chkreginfo(form_reg,6)">
                    <font color="#FF0000">*</font>&nbsp;<font color="#999999"><div id="chknew_gtel" style="color:#FF0000"></div></font></td>
          </tr>
		 
                <tr>
                  <td height="30"><div align="right">移动电话：</div></td>
                  <td height="30" colspan="2">&nbsp;<input type="text" name="mtel" size="20" class="inputcss" onBlur="chkreginfo(form_reg,5)">
                    <font color="#FF0000">*</font>&nbsp;
                  <div id="chknew_mtel" style="color:#FF0000"></div>                    <div align="right"></div></td>
                </tr>
                <tr>
                  <td height="30">&nbsp;</td>
                  <td width="150" height="30"><input type="image"  src="images/form (2).jpg">
    &nbsp;&nbsp;</td>
                  <td width="343"><img src="images/form.jpg" width="72" height="26" onClick="form_reg.reset()" style="cursor:hand"/></td>
                </tr>
        </table>
</form></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
</table>
</body>
</html>