import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  // here tell us what selectors we need otuse to add it to another 
  // component. Everything is prefixed with app
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
  
  // Angular provides 2 way binding which means
  // we can bind data/properties in our component
  // and display them in our templates
  // we can take data from a form and display
  // them in our component
export class NavComponent implements OnInit {
  // her we create a class property to store 
  // what user enters into the form
  model: any = {}
  loggedIn: boolean;
  // below we will inject our service 
  // inside the component
  // ***** remember to make whats inside the constructor
  // private ******
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }
  // we gonna use account service to log in
  // this login method is returning an observable of model
  // we need to sub to the observable
  login()
  {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response)
      this.loggedIn = true;
    }, error => {
      console.log(error)
    })
  }

  // here we can provide a logout method

  logout()
  {
    this.accountService.logout();
    this.loggedIn = false;
  }
  //bevause we are getting the currentuser here we want to ad this to our ngOnIt
  // this helps with persistent
  // logins
  getCurrentUser()
  {
    // the double !! turns the user into a bool
    this.accountService.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    }, error => {
      console.log(error);
    })
  }

}
