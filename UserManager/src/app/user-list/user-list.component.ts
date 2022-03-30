import { Component, OnInit } from '@angular/core';
import { User } from '../shared/user.model';
import { UserService } from '../shared/user.service';
import { Routes, RouterModule, Router } from "@angular/router";
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit { 
  ListUser:any=[];
  RowID:number = 1;
  constructor(private service:UserService,private router: Router) { }

  User:any;

  ngOnInit(): void {
    // this.getList();
  }

  // getList(){
  //   this.service.getListUser().subscribe(data=>{
  //     this.ListUser=data; 
  //   });
  // } 

  
  EditUser(ID:string){
    this.router.navigate(
      ['/Edit'],
      { queryParams: { ID: ID } }
    );
  }

  DeleteUser(ID:string){
    this.service.DeleteUser(ID).subscribe(data=>{
      this.ListUser=data; 
    });
  } 
}
