import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';  
import { Routes, RouterModule, Router } from "@angular/router";
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent implements OnInit {

  constructor(private service:UserService,private router: Router,private formgroup:FormBuilder ) { } 
  EmailRegex = '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}$';
  PhoneNumberRegex = '^([84|0]+(3|5|7|8|9|1[2|6|8|9]))+([0-9]{8})$';
 
  EmailValidate = "";
  PhoneNumberValidate = "";
  BirthDateValidate = "";
  PasswordConfirmValidate = ""; 

  UserForm = this.formgroup.group({
  UserName: new FormControl(''),
  BirthDate: new FormControl(''),
  Gendel: new FormControl(''), 
  PhoneNumber: new FormControl(''), 
  Email: new FormControl(''), 
  Password: new FormControl(''),
  PasswordConfirm: new FormControl(''),
});

get validator() {
  return true;
}
  ngOnInit(): void { 
    var islogged = localStorage.getItem('islogged');
    if(islogged == null|| islogged == "")
    this.router.navigate(['/Login']);
    this.CheckPhoneNumber();
    this.CheckAge();
    this.ComparePassword();
  } 

  CheckEmail(){ 
    if(this.UserForm.value.Email.match(this.EmailRegex) == null && this.UserForm.value.Email != "")
    { 
      this.EmailValidate = "* Email không đúng định dạng";
    } 
    else
    {
      this.EmailValidate ="";
    } 
  } 

  CheckPhoneNumber(){ 
    if(this.UserForm.value.PhoneNumber === ''){
      this.PhoneNumberValidate ="* Số điện thoại bắt buộc nhập!";
    }
    else if(this.UserForm.value.PhoneNumber.match(this.PhoneNumberRegex) == null)
    { 
      this.PhoneNumberValidate = "* Số điện thoại không đúng định dạng";
    } 
    else
    {
      var PhoneNumberStart = this.UserForm.value.PhoneNumber.substring(0,2);
      var PhoneNumberEnd = this.UserForm.value.PhoneNumber.substring(2,11); 
      console.log(PhoneNumberStart);
      if(PhoneNumberStart == 84){
        this.UserForm.value.PhoneNumber = 0 + PhoneNumberEnd;
      } 
      this.PhoneNumberValidate ="";
    } 
  } 

  CheckAge(){  
    var BirthDate =new Date(this.UserForm.value.BirthDate) ;  
    var Now = new Date();
    var ageyear = Now.getFullYear()- BirthDate.getFullYear() ;
     
    if(this.UserForm.value.BirthDate === ''){
      this.BirthDateValidate ="* Ngày sinh bắt buộc nhập!";
    } 
    else if(ageyear < 18){
      this.BirthDateValidate ="* Nhân viên phải >= 18 tuổi!";
    } 
    else
    {
      this.BirthDateValidate ="";
    } 
  } 

  ComparePassword(){  
    if(this.UserForm.value.PasswordConfirm === ''){
      this.PasswordConfirmValidate ="* Mật khẩu xác nhận bắt buộc nhập!";
    } 
    else if(this.UserForm.value.PasswordConfirm != this.UserForm.value.Password){
      this.PasswordConfirmValidate ="* Mật khẩu xác nhận không khớp!";
    }  
    else
    {
      this.PasswordConfirmValidate ="";
    } 
  } 
  onSubmit(): void {  
    var Error = ""; 
    document.querySelectorAll(".errorMessage").forEach(element => {
      Error = Error + element.innerHTML;
    }); 
    console.log(Error);
     if(Error.trim() === ""){
      this.service.CreateUser(this.UserForm.value).subscribe(data=>{ 
       alert(data.Title); 
      
       if(data.IsError == false){ 
        this.router.navigate(['/Index']); 
       }
       
      });
     } 
   }
   ReturnIndex(){ 
      if(confirm("Quay về trang quản trị?")) 
      this.router.navigate(['/Index']);
   } 
   Logout() {
    if (confirm("Đăng xuất?"))
      this.router.navigate(['/Login']);
      localStorage.removeItem("islogged");
  }
}
