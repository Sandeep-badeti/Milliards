import { Component } from '@angular/core';
import { AuthenticationService } from '../login/authentication.service';
import { User } from '../login/users';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from '../constants/app.constants';
@Component({
  selector: 'app-header-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  showMenu: boolean = false;
  isExpanded: boolean = false;
  user: User;
  activePath: string = AppConstants.ActivePath;

  constructor(private authService: AuthenticationService,
    private transalteService: TranslateService,
    private router: Router) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit(): void {
    this.user = new User;
    this.authService.currentUser.subscribe(data => {
      if (data && data.token != undefined && data.token != null) {
        this.user = data;
        this.showMenu = true;
      } else {
        this.showMenu = false;
      }
    });
  }

  logout() {
    this.authService.logout().subscribe(data => {
      //logout
    }, error => {
      //this.authService.clearStorage();
    })
  }
  redirect(path: string) {
    this.activePath = path;
    this.router.navigate([path]);
  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

}
