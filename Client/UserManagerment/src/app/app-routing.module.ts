import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { UserListComponent } from './user-list/user-list.component';

 
const routes: Routes = [
  {path:'',component:LoginComponent},
  {path : "Login",component: LoginComponent},
  {path : "UserList",component: UserListComponent},
  {path : "Index",component: IndexComponent},
  {path : "Edit",component: EditComponent},
  {path : "Create",component: CreateComponent},
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
