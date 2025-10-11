import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NewMasterComponent } from './new-master/new-master.component';
import { PasswordManagerComponent } from './password-manager/password-manager.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { EditPasswordComponent } from './edit-password/edit-password.component';
import { EditDBComponent } from './edit-db/edit-db.component';

export const routes: Routes = [
    {path:"login",component:LoginComponent},
    {path:"new",component:NewMasterComponent},
    {path:"manager",component:PasswordManagerComponent},
    {path:"newPass",component:NewPasswordComponent},
    {path:"edit/:id",component:EditPasswordComponent},
    {path:"config",component:EditDBComponent}
];
