import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { UserService } from '../shared/user.service'; 
@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css'], 
})
export class IndexComponent implements OnInit {
  LstUser:any=[]; 
  TotalPage:any=[]; 
  RowNumber:number = 0;
  PreviousPage:number = 0;
  NextPage:number = 0; 
  TotalRecord:number = 0; 
  public CurrentPage = 1; 
  
  public SearchText ="";
  public Gendel = ""; 
  public StartDate = ""; 
  public EndDate = ""; 


  public Validate ="";
  
  constructor(private service:UserService,private router: Router) {}
  
  ngOnInit(): void {
    var islogged = localStorage.getItem('islogged');
    if(islogged == null|| islogged == "")
    this.router.navigate(['/Login']);
    this.Search();
  }
   
  Search(){
    this.service.getListUser(this.SearchText,this.StartDate,this.EndDate,this.Gendel,this.CurrentPage).subscribe(data=>{
      if(data.IsError == true)
      {
        this.Validate = data.Title;
        alert( data.Title);
      }
      this.LstUser = data.ObjData.LstData;  
      this.RowNumber = data.ObjData.RowNumber;   
      this.TotalPage = data.ObjData.TotalPage;   
    });
  }

  SetPage(Page:number){ 
    this.CurrentPage = Page;
    this.Search();
  }

  CreateUser(){
    this.router.navigate(
      ['/Create'] 
    ); 
  }

  EditUser(ID:string){
    this.router.navigate(
      ['/Edit'],
      { queryParams: { ID: ID } }
    );
  }

  DeleteUser(ID:string){
    if(confirm("Chấp nhận xóa người dùng?")) {
      this.service.DeleteUser(ID).subscribe(data=>{
        alert(data.Title);
        this.Search();
      });
    } 
  }
  Logout() {
    if (confirm("Đăng xuất?"))
      this.router.navigate(['/Login']);
      localStorage.setItem("islogged","");
      localStorage.removeItem("islogged");
  }
}
