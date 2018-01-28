function checkregtel(regtel){
	var str=regtel;
	var Expression=/^13(\d{9})$|^15(\d{9})$/;
	var objExp=new RegExp(Expression);
	if(objExp.test(str)==true){
		return true;
	}else{
		return false;
	}
}
function checkregtels(regtels){
	var str=regtels;
	var Expression=/^(\d{3}-)(\d{8})$|^(\d{4}-)(\d{7})$|^(\d{4}-)(\d{8})$/;
	var objExp=new RegExp(Expression);
	if(objExp.test(str)==true){
		return true;
	}else{
		return false;
	}
}
function checkregemail(emails){
	var str=emails;
	var Expression=/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
	var objExp=new RegExp(Expression);
	if(objExp.test(str)==true){
		return true;
	}else{
		return false;
	}
}

function chkreginfo(form,mark){
  if(mark==0 || mark=="all"){
   	 if(form.recuser.value==""){
	   chknew_recuser.innerHTML="请输入用户名！";
	   form.recuser.style.backgroundColor="#FF0000";
	   return false;
   	 }else{
   	   chknew_recuser.innerHTML="";
	   form.recuser.style.backgroundColor="#FFFFFF";
   	 }
   }


  if(mark==1 || mark=="all"){
   	 if(form.address.value==""){
	   chknew_address.innerHTML="请输入联系地址！";
	   form.address.style.backgroundColor="#FF0000";
	   return false;
   	 }else{
   	   chknew_address.innerHTML="";
	   form.address.style.backgroundColor="#FFFFFF";
   	 }
   }


 if(mark==2 || mark=="all"){
   	 if(form.postalcode.value==""){
	   chknew_postalcode.innerHTML="请输入邮编！";
	   form.postalcode.style.backgroundColor="#FF0000";
	   return false;
   	 }else if(isNaN(form.postalcode.value)){
   	   chknew_postalcode.innerHTML="邮编由数字组成！";
	   form.postalcode.style.backgroundColor="#FF0000";
	   return false;
	 }else if(form.postalcode.value.length!=6){
   	   chknew_postalcode.innerHTML="邮编由6位数字组成！";
	   form.postalcode.style.backgroundColor="#FF0000";
	   return false;
   	 }else{
   	   chknew_postalcode.innerHTML="";
	   form.postalcode.style.backgroundColor="#FFFFFF";
   	 }
   }


   if(mark==3 || mark=="all"){
   	 if(form.qq.value==""){
	   chknew_qq.innerHTML="请输入QQ号码！";
	   form.qq.style.backgroundColor="#FF0000";
	   return false;
   	 }else if(isNaN(form.qq.value)){
   	   chknew_qq.innerHTML="QQ号由数字组成！";
	   form.qq.style.backgroundColor="#FF0000";
	   return false;
   	 }else{
   	   chknew_qq.innerHTML="";
	   form.qq.style.backgroundColor="#FFFFFF";
   	 }
   }

  if(mark==4 || mark=="all"){
   	 if(form.email.value==""){
	   chknew_email.innerHTML="请输入E-mail地址！";
	   form.email.style.backgroundColor="#FF0000";
	   return false;
   	 }else if(!checkregemail(form.email.value)){
	   		chknew_email.innerHTML="邮箱地址的格式不正确！";
	   		form.email.style.backgroundColor="#FF0000";
	   		return false;
	   }else{
	   	  chknew_email.innerHTML="";
	      form.email.style.backgroundColor="#FFFFFF";
	   }
   }

  if(mark==5 || mark=="all"){
		if(form.mtel.value==""){
	   		chknew_mtel.innerHTML="请输入电话号码！";
	   		form.mtel.style.backgroundColor="#FF0000";
	   		return false;
   	 	}else if(!checkregtel(form.mtel.value)){
	   		chknew_mtel.innerHTML="电话号码的格式不正确！";
	   		form.mtel.style.backgroundColor="#FF0000";
	   		return false;
   	 	}else if(isNaN(form.mtel.value)){
   	   		chknew_mtel.innerHTML="电话号由数字组成！";
	   		form.mtel.style.backgroundColor="#FF0000";
	   		return false;
   	 	}else{
   	   		chknew_mtel.innerHTML="";
	   		form.mtel.style.backgroundColor="#FFFFFF";
   	 	}
   	}


   if(mark==6 || mark=="all"){
		if(form.gtel.value==""){
	   		chknew_gtel.innerHTML="请输入电话号码！";
	   		form.gtel.style.backgroundColor="#FF0000";
	   		return false;
   	 	}else if(!checkregtels(form.gtel.value)){
	   		chknew_gtel.innerHTML="电话号码的格式不正确！";
	   		form.gtel.style.backgroundColor="#FF0000";
	   		return false;
   	 	}else{
   	   		chknew_gtel.innerHTML="";
	   		form.gtel.style.backgroundColor="#FFFFFF";
   	 	}
   	}


   }


