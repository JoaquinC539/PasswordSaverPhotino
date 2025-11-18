import { Component, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { LoginStateService } from '../services/loginState.service';
import { PasswordService } from '../services/password.service';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';

@Component({
  selector: 'app-edit-db',
  imports: [ReactiveFormsModule,RouterLink,ScreenLoaderComponent],
  templateUrl: './edit-db.component.html',
  styleUrl: './edit-db.component.scss'
})
export class EditDBComponent implements OnInit{
  errorMessage=signal<string>("")
  loading=signal<boolean>(false);
  dbForm=new FormGroup({
    dbFile:new FormControl<string>("")
  })
  backFrom=signal("");

  constructor(private passwordService:PasswordService,private router:Router,private route:ActivatedRoute){}
  ngOnInit(): void {
    this.route.queryParams.subscribe((params)=>{
      this.backFrom.set(params["from"]);
    });
  }


  async selectDB(event:SubmitEvent):Promise<void>{
    event.preventDefault();
    this.errorMessage.set("");    
    const filePath=this.dbForm.value.dbFile;
    if(filePath){
      this.loading.set(true);
      await this.passwordService.logout();
      const res = await this.passwordService.changeDB(filePath);
      if(!res){
        this.errorMessage.set("File not existant or not valid, the database wasn't updated");
        this.loading.set(false);
        return;
      }
      const count = await this.passwordService.getMasterCount()
    if(!count){
      this.router.navigate(["/new"])
    }else{
      this.router.navigate(["/login"])
    }
      this.loading.set(false);
    }else{
      this.errorMessage.set("DB path cannot be empty");
    }
  }
}
