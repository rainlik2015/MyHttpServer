<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
        <title>隐藏PHP扩展名</title>
    </head>
    <style type="text/css">
        <!--
        .style2 {
            color: #000000;
            font-size: 9pt;
        }
        .style3 {
            font-size: 9pt;
            font-weight: bold;
        }
        .style4 {font-size: 9pt}
        -->
    </style>

    <body>
    <table width="1014" height="706" border="0" align="center" cellpadding="0" cellspacing="0" background="images/form1.jpg">
          <tr>
            <td width="175" height="236">&nbsp;</td>
            <td width="501">&nbsp;</td>
            <td width="338">&nbsp;</td>
          </tr>
          <tr>
            <td height="175">&nbsp;</td>
            <td valign="top"><table width="88%"  border="0" cellspacing="-2" cellpadding="-2">
                        
                        <tr>
                            <td width="18%" height="22" align="center"><span class="style4">用 户 名：</span></td>
                            <td width="82%" class="style4">&nbsp;<?php echo $_POST[recuser]; ?></td>
                        </tr>
                        <tr>
                            <td height="22" align="center"><span class="style4">地 址：</span></td>
                            <td height="28" class="style4">&nbsp;<?php echo $_POST[address];?></td>
                        </tr>
                        <tr>
                            <td height="22" align="center"><span class="style4">邮编：</span></td>
                            <td height="28" class="style4">&nbsp;<?php echo $_POST[postalcode];?></td>
                        </tr>
                        <tr>
                            <td height="22" align="center"><span class="style4">QQ：</span></td>
                            <td class="style4">&nbsp;<?php echo $_POST[qq];?></td>
                        </tr>
                        <tr>
                            <td height="22" align="center" style="padding-left:10px"><span class="style4">Email：</span></td>
                            <td class="style4">&nbsp;<?php echo $_POST[email];?></td>
                        </tr>
                        <tr>
                          <td height="22" align="center" class="style4" style="padding-left:10px">手机：</td>
                          <td class="style4"><?php echo $_POST[mtel];?></td>
                        </tr>
                        <tr>
                          <td height="22" align="center" class="style4" style="padding-left:10px">电话：</td>
                          <td class="style4"><?php echo $_POST[gtel];?><strong></strong></td>
                        </tr>
            </table></td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td align="center" valign="top">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
    </table>
    </body>
</html>
