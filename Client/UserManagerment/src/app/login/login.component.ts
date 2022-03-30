import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';
import { Routes, RouterModule, Router } from "@angular/router";
// import { TokenStorageService } from '../shared/token-storage.service'; 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
 
  public username ="";
  public password ="";
  public validate ="";
form: any = {
  username: null,
  password: null
};
 
 
  constructor(private service:UserService,private router: Router) { }
  ngOnInit(): void {
    
  }

  onSubmit(): void { 
   this.form.username = this.username;
   this.form.password = this.password;
   this.service.Login(this.form).subscribe(data=>{
     if(data.IsError === false)
     {
      localStorage.setItem("islogged",data.ObjData.TOKEN); 
        this.router.navigate(
          ['/Index']  ); 
      } 
    this.validate = data.Title;
    console.log(data.IsError);
   });
  } 
}
