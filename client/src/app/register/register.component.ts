import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from 'app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model : any = {}
//to get info from a parent(Home) to child component (Register) we need an Input decorator here.
//@Input() usersFromHomeComponent : any;
//to emit info from child(Register) to parent(child) component we need Output decorator here.
@Output() cancelRegister = new EventEmitter();

constructor(private accountService : AccountService) { 
}

ngOnInit() : void {}

register() {
  this.accountService.register(this.model).subscribe({
    next : () =>{
      this.cancel();
    },
    error: error=> console.log(error)
  })
}
cancel(){
  this.cancelRegister.emit(false);
  //we can emit anything here, but in this case we are passing false to turn off the registerMode in the home component
}



}
