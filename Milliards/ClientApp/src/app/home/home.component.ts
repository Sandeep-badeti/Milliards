import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../login/authentication.service';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from '../constants/app.constants';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  message: string = '';
  status: boolean = false;
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
    private authService: AuthenticationService,
    private transalteService: TranslateService,
    private router: Router) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    const currentUser = this.authService.currentUserValue;
    if (currentUser && currentUser.token != undefined && currentUser.token != null) {
      this.router.navigate([AppConstants.ProductListUrl]);
    } 
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
    
  }

  login(frm = null) {
    this.authService.login(this.loginForm.value).pipe(first()).subscribe(
      data => {
        this.status = true;
        this.router.navigate([AppConstants.ProductListUrl]);
      },
      error => {
        console.log("Login Error---------");
        console.log(error);
        this.message = "Invalid Username or Password";
      }
    );
  }
  clearError() {
    this.message = "";
  }
}
