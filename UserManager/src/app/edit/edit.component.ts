import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../shared/user.service';
import { ActivatedRoute, Router } from "@angular/router";
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { User } from '../shared/user.model';
 
@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {

  constructor(private service: UserService, private router: Router, private activatedroute: ActivatedRoute, private formgroup: FormBuilder ) { }
  EmailRegex = '^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$';
  PhoneNumberRegex = '^([84|0]+(3|5|7|8|9|1[2|6|8|9]))+([0-9]{8})$';

  EmailValidate = "";
  PhoneNumberValidate = "";
  BirthDateValidate = "";
  User: User = new User();

  @ViewChild('form') form!: NgForm;

  get validator() {
    return true;
  }
  
  ngOnInit(): void {
    this.User.ID = this.activatedroute.snapshot.queryParamMap.get('ID') + "";
    this.GetUser(this.User.ID); 
  }

  GetUser(ID: string) {
    this.service.getUser(this.User.ID).subscribe(data => {  
      this.User = data.ObjData;    
      console.log(this.User);
    });
  }

  CheckEmail() {
    if (this.User.Email.match(this.EmailRegex) == null && this.User.Email != "") {
      this.EmailValidate = "* Email không đúng định dạng";
    }
    else {
      this.EmailValidate = "";
    }
  }

  CheckPhoneNumber() {
    if (this.User.PhoneNumber === '') {
      this.PhoneNumberValidate = "* Số điện thoại bắt buộc nhập!";
    }
    else if (this.User.PhoneNumber.match(this.PhoneNumberRegex) == null) {
      this.PhoneNumberValidate = "* Số điện thoại không đúng định dạng";
    }
    else {
      var PhoneNumberStart = this.User.PhoneNumber.substring(0, 2);
      var PhoneNumberEnd = this.User.PhoneNumber.substring(2, 11); 
      if (PhoneNumberStart == "84") {
        this.User.PhoneNumber = "0" + PhoneNumberEnd;
      }
      this.PhoneNumberValidate = "";
    }
  }

  CheckAge() {
    var BirthDate = new Date(this.User.BirthDateConvert);
    var Now = new Date();
    var ageyear = Now.getFullYear() - BirthDate.getFullYear();

    if (this.User.BirthDate === '') {
      this.BirthDateValidate = "* Ngày sinh bắt buộc nhập!";
    }
    else if (ageyear < 18) {
      this.BirthDateValidate = "* Nhân viên phải >= 18 tuổi!";
    }
    else {
      this.BirthDateValidate = "";
    }
  }

  onSubmit(): void { 
    console.log(this.User);
    var Error = "";
    document.querySelectorAll(".errorMessage").forEach(element => {
      Error = Error + element.innerHTML;
    });
    console.log(Error);
    if (Error.trim() === "") {
      this.service.UpdateUser(this.User).subscribe(data => {
        alert(data.Title); 
        if (data.IsError == false) {
          this.router.navigate(['/Index']);
        }
      });
    }
  }
  ReturnIndex() {
    if (confirm("Quay về trang quản trị?"))
      this.router.navigate(['/Index']);
  }
  Logout() {
    if (confirm("Đăng xuất?"))
      this.router.navigate(['/Login']);
      localStorage.setItem("islogged","")
  }
}
