import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { AuthService } from "../_services/auth.service";
import { error } from "util";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"]
})
export class RegisterComponent implements OnInit {
 // @Input() valuesFromRegister:any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor( private authService: AuthService) {}

  ngOnInit() {}
  register () {
   // console.log("تم الاشتراك ");
   // console.log(this.model);
   this.authService.register(this.model).subscribe(
     ()=>{console.log('تم الاشتراك')},error=>{console.log(error)}
   )
  }
  cancel() {
    console.log(" ليس الأن ");
    this.cancelRegister.emit(false);
  }
}
